//botões
var btn_acesso = document.getElementById('btn_acesso');
var btn_visualizacao = document.getElementById('btn_visualizacao');
var btn_export = document.getElementById('btn_export');
//inputs
var domain = document.getElementById('domain');
var ds_geo = document.getElementById('ds_geo');
var ds_dados = document.getElementById('ds_dados');
//label
var lb_avisos = document.getElementById('lb_avisos');
var lb_contador = document.getElementById('lb_rows_contador');
//dropdown list
var select_dados = document.getElementById('select_dados');
var select_geo = document.getElementById('select_geo');

//EventListeners dos buttones

btn_acesso.addEventListener("click", function(e){
    //Filtro dos dados
    /*
    if(domain.value.length < 9){
        alert('DomÃ­nio invÃ¡lido');
        domain.focus();
        return false;
    }
    if(ds_geo.value.length < 9){
        alert('Valores invÃ¡lidos');
        ds_geo.focus();
        return false;
    }
    if(ds_dados.value.length < 9){ 
        lb_avisos.innerHTML = "ID do dataset de dados invÃ¡lido.";
        ds_dados.focus();
        return false;
    }
    */
    //Reseta o label de lb_avisos
    lb_avisos.innerHTML = "";
    var socrata = new soda.Consumer(domain.value);
    //Requisição das colunas
    //DADOS GEOGRÁFICOS
    socrata.query().withDataset(ds_geo.value).getRows()
        .on('success', function(rows){
            //Popula o select
            clearSelect(select_dados);
            preencheSelect(ds_geo.value, select_geo);
        })
        .on('error', function(error){
            console.log(error);
            alert("Dataset de dados invÃ¡lido");
        });
    //Requisição das colunas
    //DADOS CONTÁVEIS
    socrata.query().withDataset(ds_dados.value).getRows()
    .on('success', function (rows) {
        //Popula o select
        clearSelect(select_dados);
        preencheSelect(ds_dados.value, select_dados);
    })
    .on('error', function (error) {
        console.log(error);
        alert("Dataset de dados invÃ¡lido");
    });
});

btn_visualizacao.addEventListener("click", function (e) {
    //Reseta o label de lb_avisos
    lb_avisos.innerHTML = "";
    //Reinicio do mapa
    initMap();
    var socrata = new soda.Consumer(domain.value);
    //DADOS GEOGRÁFICOS
    //Requisição dos dados
    socrata.query().withDataset(ds_geo.value).select('the_geom').getRows()
        .on('success', function (rows) {
            //Desenha o mapa
            var coordenadas = [];
            var cor = 'grey';
            for (var k = 0; k < rows.length; k++) {
                //Pega as coordenadas de todos os poligonos
                coordenadas = rows[k].the_geom.coordinates[0][0];
                var pontos = [];
                //Define os pontos para cada poligono
                for (var i = 0; i < coordenadas.length; i++) {
                    var c = coordenadas[i];
                    pontos.push({ lat: c[1], lng: c[0] });
                }
                var linearRing = new google.maps.Data.LinearRing(pontos);
                //Instância o Polygon  
                var poligono = new google.maps.Data.Polygon([linearRing]);
                //Adiciona o Polygon a DataLayer
                /*var n = k % 3;
                switch (n) {
                    case 0:
                        cor = 'red';
                        break;
                    case 1:
                        cor = 'green';
                        break;
                    case 2:
                        cor = 'blue';
                        break;
                }*/
                map.data.add({ geometry: poligono, properties: { 'color': cor } });
            }
            //Altera o centro
            map.setCenter({ lat: coordenadas[0][1], lng: coordenadas[0][0] })
            //Popula o select
            clearSelect(select_geo);
            preencheSelect(ds_geo.value, select_geo);
            //Preenche os avisos
            lb_avisos.innerHTML = "Mapa Carregado";
            lb_contador.innerHTML = rows.length + " dados carregados";

        })
        .on('error', function (error) {
            console.log(error);
            lb_avisos.innerHTML = "Dataset geográfico invÃ¡lido";
            return false;
        });
    //DADOS CONTÁVEIS
    //Reqisição dos dados

});

btn_export.addEventListener('click', function (e) {
    map.data.toGeoJson(function (obj) {
        console.log(JSON.stringify(obj));
        var file = new Blob([JSON.stringify(obj)], { type: "aplication/json" });
        saveAs(file,"mapa.json");
    })
});

//Metodos auxiliares

function clearSelect(select){
    var i;
    for(i = select.childElementCount - 1 ; i >= 0 ; i--)
    {
        select.remove(i);
    }
}

function preencheSelect(dataset, select){
    //Preenche o select com uma requisição http a api que contém os metadados
    var request = new XMLHttpRequest();
    url = "https://" + domain.value + "/api/views/" + dataset;
    console.log(url);
    //Quando a requisição estiver completada
    request.onreadystatechange = function () {
        //Testa se foi tudo bem
        if (request.readyState === 4 && request.status === 200){
            var c = JSON.parse(request.responseText);
            c.columns.forEach(function(element, index, array ){
                if(element.id !== -1){
                    opt = document.createElement("option");
                    opt_text = document.createTextNode(element.name);
                    opt.setAttribute('value',element.fieldName);
                    opt.appendChild(opt_text);
                    select.appendChild(opt);
                }
            });
        }
        else {
            avisos.innerHTML = "Requisição falhou. Verifique Endereço e Endpoint";
            console.log(request.status);
        }
    };
    request.open("GET", url, true);
    request.send();
}

function initMap() {
    //Inicializa o mapa na div 'map'.
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 10,
        center: { lat: 41.84173095, lng: -87.67467499 },
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    map.data.setStyle(function (feature) {
        return {
            strokeWeight: 1,
            fillColor: feature.getProperty('color')
        };
    });
}