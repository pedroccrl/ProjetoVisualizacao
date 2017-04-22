var map;
//botões
var btn_select = document.getElementById('btn_select');
var btn_dataset = document.getElementById('btn_dataset');
var btn_export = document.getElementById('btn_export');
//inputs
var domain = document.getElementById('domain');
var ds = document.getElementById('ds');
//label
var avisos = document.getElementById('avisos');
var contador = document.getElementById('rows_contador');
var lb_nome_dataset = document.getElementById('lb_nome_dataset');
//dropdown list
//var select_long = document.getElementById('select_long');
//var select_lat = document.getElementById('select_lat');
var select_loc = document.getElementById('select_loc');
//checkbox
var cb_inverter = document.getElementById('cb_inverter');

btn_dataset.addEventListener("click", function (e) {
    //Popula os selects
    /*
    clearSelect(select_lat);
    clearSelect(select_long);
    preencheSelect(ds.value, select_lat);
    preencheSelect(ds.value, select_long);
    */
    clearSelect(select_loc);
    preencheSelect(ds.value, select_loc, lb_nome_dataset);
});


btn_select.addEventListener("click", function (e) {
    //Filtro dos dados
    if (domain.value.length < 9) {
        alert('DomÃ­nio invÃ¡lido');
        domain.focus();
        return false;
    }
    if (ds.value.length < 9) {
        alert('Valores invÃ¡lidos');
        ds.focus();
        return false;
    }
    //Reseta o label de avisos
    avisos.innerHTML = "Carregando coordenadas";
    //Reinicio do mapa
    initMap();
    var socrata = new soda.Consumer(domain.value);
    //Recupera os dados geográficos
    socrata.query().withDataset(ds.value).select().limit(100000).getRows()
        .on('success', function (rows) {
            var coordenadas = [];
            var erros = 0;
            //Desenha o mapa
            for (var k = 0; k < rows.length; k++) {
                try {
                    lat = rows[k][select_loc.options[select_loc.selectedIndex].value]["coordinates"][0];
                    long = rows[k][select_loc.options[select_loc.selectedIndex].value]["coordinates"][1];
                    if (cb_inverter.checked == true) {
                        var aux = lat;
                        lat = long;
                        long = aux;
                    }
                    coordenadas.push(new google.maps.LatLng(parseFloat(lat), parseFloat(long)));
                }
                catch (err) {
                    erros++;
                }
            }
            console.log(erros);
            map.setCenter(coordenadas[0]);
            var heatmap = new google.maps.visualization.HeatmapLayer({
                data: coordenadas,
                map: map
            });
            avisos.innerHTML = "Mapa Carregado";
            contador.innerHTML = rows.length + " dados carregados";
            map.data.add(heatmap);

        })
        .on('error', function (error) {
            console.log(error);
            avisos.innerHTML = "Dataset geográfico invÃ¡lido";
            return false;
        });
    

});

btn_export.addEventListener('click', function (e) {
    map.data.toGeoJson(function (obj) {
        console.log(JSON.stringify(obj));
        var file = new Blob([JSON.stringify(obj)], { type: "aplication/json" });
        saveAs(file, lb_nome_dataset.innerText + ".json");
    });
});


function clearSelect(select) {
    var i;
    for (i = select.childElementCount - 1 ; i >= 0 ; i--) {
        select.remove(i);
    }
}

function preencheSelect(dataset, select, label) {
    //Preenche o select com uma requisição http a api que contém os metadados
    var request = new XMLHttpRequest();
    url = "https://" + domain.value + "/api/views/" + dataset;
    console.log(url);
    //Quando a requisição estiver completada
    request.onreadystatechange = function () {
        //Testa se foi tudo bem
        if (request.readyState === 4 && request.status === 200) {
            var c = JSON.parse(request.responseText);
            //Seta o nome do dataset na pagina
            lb_nome_dataset.innerHTML = c.name;

            c.columns.forEach(function (element, index, array) {
                //Para cada elemento
                if (element.id !== -1 && element.dataTypeName == "point") {
                    var opt = document.createElement("option");
                    opt_text = document.createTextNode(element.name); 
                    opt.setAttribute('value', element.fieldName);
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
}