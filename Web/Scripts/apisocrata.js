var dados;

function loadData()
{
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function ()
    {
        if (xhttp.readyState == 4 && xhttp.status == 200)
        {
            dados = JSON.parse(xhttp.responseText);
            for (var i = 0; i < dados.length; i++) {
                document.getElementById("content").innerHTML += "<br>Nome da Área: ";
                document.getElementById("content").innerHTML += dados[i].community_area_name;
                document.getElementById("content").innerHTML += "<br> Renda per capita: R$";
                document.getElementById("content").innerHTML += dados[i].per_capita_income_;
            }
            
        }
    };
    xhttp.open("GET", "https://data.cityofchicago.org/resource/jcxq-k9xf.json?$select=*", true);
    xhttp.send();
}