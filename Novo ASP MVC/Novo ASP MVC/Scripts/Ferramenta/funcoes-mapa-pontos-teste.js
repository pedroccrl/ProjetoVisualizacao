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
var select_api = document.getElementById('select_api');
var select_loc = document.getElementById('select_loc');
//checkbox
var cb_inverter = document.getElementById('cb_inverter');

btn_dataset.addEventListener("click", function (e) {
    //A SER ALTERADO
    clearSelect(select_loc);
    //Popula os selects
    if (select_api.options[select_api.selectedIndex].value == "Socrata")
        preencheSelect(select_loc, lb_nome_dataset);
    else
        btn_select.click();
});


btn_select.addEventListener("click", function (e) {
    //Reseta o label de avisos
    avisos.innerHTML = "Carregando coordenadas";
    contador.innerHTML = "";
    //Reinicio do mapa
    initMap();
    //Cria a requisição
    var request = new XMLHttpRequest();
    if (select_api.options[select_api.selectedIndex].value == "Socrata")
        coluna = select_loc.options[select_loc.selectedIndex].value;
    request.onreadystatechange = function () {
        var rows = JSON.parse(request.responseText);
        //Adapta para aceitar GeoJSON
        if (isGeoJSON(rows))
            rows = rows["features"];
        var ponto;
        var erros = 0;
        //Desenha o mapa
        for (var k = 0; k < rows.length; k++) {
            try {
                if (select_api.options[select_api.selectedIndex].value == "Socrata"){
                    dados = rows[k][coluna];
                }
                else//Se for acessivel via CKAN
                    //Só está aceitando GeoJSON neste caso
                    dados = rows[k]["geometry"]; //Se o arquivo estiver em GeoJSON, ele deve ter o atributo geometry
                //Versão antiga do Socrata utilizava este formato para pontos;
                if (dados["type"] == "point" || dados["type"] == "Point") {
                    lat = dados["coordinates"][0];
                    long = dados["coordinates"][1];
                    if (cb_inverter.checked == true) {
                        var aux = lat;
                        lat = long;
                        long = aux;
                    }
                }
                //Formato atual implementado pelo Socrata
                else {
                    lat = dados["latitude"];
                    long = dados["longitude"];
                }
                ponto = new google.maps.Data.Point({ lat: parseFloat(lat), lng: parseFloat(long) });
                map.data.add({ geometry: ponto });

            }
            catch (err) {
                console.log(err);
                erros++;
            }
        }
        console.log("erros: " + erros);
        map.setCenter({lat: parseFloat(lat), lng: parseFloat(long)});
        avisos.innerHTML = "Mapa Carregado";
        contador.innerHTML = rows.length + " dados carregados";

    }
    if (select_api.options[select_api.selectedIndex].value == "Socrata")
        request.open("GET", domain.value + '?$limit=10000&$select=' + coluna, true);
    else
        request.open("GET", domain.value, true);
    request.send();
    

});

btn_export.addEventListener('click', function (e) {
    map.data.toGeoJson(function (obj) {
        console.log(JSON.stringify(obj));
        var file = new Blob([JSON.stringify(obj)], { type: "aplication/json" });
        saveAs(file,lb_nome_dataset.innerText + ".json");
    });
});

function isGeoJSON(dados) {
    if (dados["type"] == "FeatureCollection")
        return true;
    else
        return false;
}

function clearSelect(select) {
    var i;
    for (i = select.childElementCount - 1 ; i >= 0 ; i--) {
        select.remove(i);
    }
}

function preencheSelect(select, label) {
    //Preenche o select com uma requisição http a api que contém os metadados
    var request = new XMLHttpRequest();
    //url = "https://" + domain.value + "/api/views/" + dataset;
    url = String.replace(domain.value,'/resource','/views');
    //Quando a requisição estiver completada
    request.onreadystatechange = function () {
        //Testa se foi tudo bem
        if (request.readyState === 4 && request.status === 200) {
            var c = JSON.parse(request.responseText);
            //Seta o nome do dataset na pagina
            lb_nome_dataset.innerHTML = c.name;

            c.columns.forEach(function (element, index, array) {
                //Para cada elemento
                if (element.id !== -1) {
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