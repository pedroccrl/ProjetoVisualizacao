var dados;
var totalBytes;
var bytes;

function progresso(evt) {
    document.getElementById("content").innerHTML = evt.loaded;
}

function loadDataInDiv(divId) {
    var xhttp = new XMLHttpRequest();
    xhttp.onprogress = progresso;
    xhttp.onreadystatechange = function ()
    {
        if (xhttp.readyState == 4 && xhttp.status == 200)
        {
            dados = JSON.parse(xhttp.responseText);
            for (var i = 0; i < dados.length; i++) 
            {
                document.getElementById(divId).innerHTML += "<br><br>Nome da Área: ";
                document.getElementById(divId).innerHTML += dados[i].community_area_name;
                document.getElementById(divId).innerHTML += "<br> Renda per capita: $";
                document.getElementById(divId).innerHTML += dados[i].per_capita_income_;
            }
        }
    };
    xhttp.open("GET", "https://data.cityofchicago.org/resource/jcxq-k9xf.json?$select=*", true);
    xhttp.send();
    totalBytes = xhttp.getResponseHeader("Content-Length");
    document.getElementById(divId).innerHTML = totalBytes;
}