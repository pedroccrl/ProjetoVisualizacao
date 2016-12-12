var btn_acesso = document.getElementById('btn_acesso');
var btn_visualizacao = document.getElementById('btn_visualizacao');
var domain = document.getElementById('domain');
var ds_geo = document.getElementById('ds_geo');
var ds_dados = document.getElementById('ds_dados');

btn_acesso.addEventListener("click", function(e){
    if(domain.value.length === 0){
            alert('Domínio inválido');
            domain.focus();
            return false;
    }
    if(ds_geo.value.length !== 9){
            alert('Valores inválidos');
            ds_geo.focus();
            return false;
    }
    //if(ds_dados.value.length != 9){
    //	alert('Valores inválidos');
    //	ds_dados.focus();
    //	return false;
    //}
    var map = new google.maps.Map(document.getElementById('map'),{
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.TERRAIN
    });
    var socrata = new soda.Consumer(domain.value);
    socrata
        .query()
        .withDataset(ds_geo.value)
        .select('the_geom')
        .getRows()
        .on('success',function(rows){
            colunas = getColunas();
            var c = colunas.meta.view.columns;
            var select_areas = document.getElementById('select_areas');
            c.forEach(function(element, index, array ){
                opt = document.createElement("option");
                opt_text = document.createTextNode(element.name);
                opt.setAttribute('value',element.fieldName);
                opt.appendChild(opt_text);
                select_areas.appendChild(opt);
            });
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
        });
});
       
function getColunas(){
    var request = new XMLHttpRequest();
    url = "https://" + domain.value + "/api/views/" + ds_geo.value + "/rows.json";
    console.log(url);
    request.open("GET", url, false);
    request.send(null);
    
    return JSON.parse(request.responseText);
}



