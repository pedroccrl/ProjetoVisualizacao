var apiKeyMaps = "AIzaSyD9UfBAMHF2QhemIgsbX52OtYs8yslpsZ8";
var dados;

<!-- Essa função era usada para gerar a tabela -->
//Faz o acesso a api restful. 
function loadDataInDiv(divId,criterio) {
    var xhttp = new XMLHttpRequest();
    var table = document.getElementById(divId);     
    var criterioAlterado = camelCase(criterio);
    changeMap(criterio);
    
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == 4 && xhttp.status == 200) {
            document.getElementById(divId).innerHTML = '<tr><th>Area</th><th>'+criterioAlterado+'</th></tr>';
            dados = JSON.parse(xhttp.responseText);
            for (var i = 0; i < dados.length; i++) {
                table.insertRow().innerHTML += "<tr><td>" + dados[i]['community_area_name'] + "</td>" + 
                                                "<td>" + dados[i][criterio]+ "</td></tr>";
            }

        }
    };  
    xhttp.open("GET", "https://data.cityofchicago.org/resource/jcxq-k9xf.json?$select=community_area_name,"+criterio+"&$order="+criterio, true);
    xhttp.send();
}

function camelCase(name){
    var palavras = name.split('_');
    //Corrige um erro no per_capita_income_
    if(palavras[length-1] == '')
        palavras = palavras.slice(0,length-1);
    //Remove a palavra 'percent' e 'of' e adiciona o '%' no final
    if (palavras[0] == 'percent'){
        if(palavras[1] == 'of')
            palavras = palavras.slice(2);
        else
            palavras = palavras.slice(1);
        palavras.push('(%)');
    }

    var retorno = "";
    for (var i =  0; i < palavras.length; i++) {
        palavras[i] = palavras[i][0].toUpperCase() + palavras[i].slice(1);
        retorno += palavras[i] + " ";
    }

    return retorno;
}
