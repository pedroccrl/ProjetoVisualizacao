//botões
var btn_acesso = document.getElementById('btn_acesso');
var btn_visualizacao = document.getElementById('btn_visualizacao');
//inputs
var domain = document.getElementById('domain');
var ds_geo = document.getElementById('ds_geo');
var ds_dados = document.getElementById('ds_dados');
//label
var avisos = document.getElementById('avisos');
//dropdown list
var select_dados = document.getElementById('select_dados');
var select_geo = document.getElementById('select_geo');

btn_acesso.addEventListener("click", function(e){
    //Filtro dos dados
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
    //Reseta o label de avisos
    avisos.innerHTML = "";
    //Reinicio do mapa
    initMap();
    var socrata = new soda.Consumer(domain.value);
    //Recupera os dados geográficos
    socrata.query().withDataset(ds_geo.value).select('the_geom').getRows()
        .on('success',function(rows){
            //Desenha o mapa
            for (var k = 0; k < rows.length; k++) {
                var poligonosAreas = [];
                var coordenadas = rows[k].the_geom.coordinates[0][0];
                for (var i = 0; i < coordenadas.length; i++) {
                    var coord = coordenadas[i];
                    poligonosAreas.push({ lat: coord[1], lng: coord[0] });
                }
                var poligonos = new google.maps.Polygon({
                        paths: poligonosAreas,
                        strokeOpacity: 0.8,
                        strokeWeight: 1,
                        fillColor: 'gray',
                        fillOpacity: 0.50,
                        map: map
                    });
                if(k === 0){
                    var primeiro_ponto = coordenadas[0];
                    console.log(primeiro_ponto);
                    map.setCenter(new google.maps.LatLng(primeiro_ponto[1],primeiro_ponto[0]));
                }
            }
            //Popula o select
            clearSelect(select_geo);
            preencheSelect(ds_geo.value, select_geo);
        })
        .on('error', function(error){
            console.log(error);
            avisos.innerHTML = "Dataset geográfico invÃ¡lido";
            return false;
        });
    //Testa para saber se existe dataset preenchedio
    if(ds_dados.value.length < 9){ 
        avisos.innerHTML = "ID do dataset de dados invÃ¡lido.";
        ds_dados.focus();
        return false;
    }
    //Recupera os dados contáveis
    socrata.query().withDataset(ds_dados.value).getRows()
        .on('success', function(rows){
            //Popula o select
            clearSelect(select_dados);
            preencheSelect(ds_dados.value, select_dados);
        })
        .on('error', function(error){
            console.log(error);
            alert("Dataset de dados invÃ¡lido");
        });
    
});

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
    url = "https://" + domain.value + "/api/views/" + dataset + "/rows.json";
    console.log(url);
    request.onreadystatechange = function(){
        if (request.readyState === 4 && request.status === 200){
            var c = JSON.parse(request.responseText).meta.view.columns;
            c.forEach(function(element, index, array ){
                if(element.id !== -1){
                    opt = document.createElement("option");
                    opt_text = document.createTextNode(element.name);
                    opt.setAttribute('value',element.fieldName);
                    opt.appendChild(opt_text);
                    select.appendChild(opt);
                }
            });
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
}