var dados;
var apiKeyMaps = "AIzaSyD9UfBAMHF2QhemIgsbX52OtYs8yslpsZ8";
var progress;

function GeocodeBairro(nome, lat, lng) {
    this.nome = nome;
    this.lat = lat;
    this.lng = lng;
}

function readKml(fileName) {
    var http = new XMLHttpRequest();
    http.onreadystatechange = function(){
        if (http.readyState == 4 && http.status == 200) {
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(http.responseText, "text/xml");
            console.log(xmlDoc);
        }
    }
}

function obterLatLonChicago(bairro, dados, i, divId) {
    var result;
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState==4 && http.status==200) {
            // console.log(http.responseText);
            result = JSON.parse(http.responseText);
            // console.log(result);
            var local =  new GeocodeBairro(bairro, result.results[0].geometry.location.lat, result.results[0].geometry.location.lng);
            // document.getElementById("content").innerHTML += "Lat: " + local.lat + " Lng: " + local.lng;
            document.getElementById(divId).innerHTML += "<br><br>Nome da Área: ";
            document.getElementById(divId).innerHTML += dados[i].community_area_name;
            document.getElementById(divId).innerHTML += "<br>Renda per capita: $";
            document.getElementById(divId).innerHTML += dados[i].per_capita_income_;
            document.getElementById(divId).innerHTML += "<br>Coordenadas: ";
            document.getElementById(divId).innerHTML += "Lat: " + local.lat + " Lng: " + local.lng;
        }
    }
    http.open("GET", "https://maps.googleapis.com/maps/api/geocode/json?address=" + bairro + ",+Chicago&key=" + apiKeyMaps, true);
    http.send();
}


function progresso(evt) {
    document.getElementById("progresso").innerHTML = progress;
}

function loadDataInDiv(divId) {
    var xhttp = new XMLHttpRequest();
    progress = -1;
    xhttp.onreadystatechange = function ()
    {
        if (xhttp.readyState == 4 && xhttp.status == 200)
        {
            dados = JSON.parse(xhttp.responseText);
            document.getElementById(divId).innerHTML = "";
            for (var i = 0; i < dados.length; i++) 
            {
                progress=i;
                obterLatLonChicago(dados[i].community_area_name, dados, i, divId);
            }
        }
    };
    xhttp.open("GET", "https://data.cityofchicago.org/resource/jcxq-k9xf.json?$select=*", true);
    xhttp.send();
}

