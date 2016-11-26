var token = 'Gqi5A6FBGKP1Sn7PZeJPw7mAR';
var consumer = new soda.Consumer('data.cityofchicago.org');
var consumerColor = new soda.Consumer('data.cityofchicago.org');
var map;

function initMap() {
    //Inicializa o mapa na div 'map'.
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 10,
        center: { lat: 41.84173095, lng: -87.67467499},
        mapTypeId: google.maps.MapTypeId.TERRAIN
    });
}

function changeMap(criterio) {
    //Primeiro ele acessa para pegar o nome e a renda do local.
    //Repare que a função chamada aqui é assincrona.
    //O que acontece é que em algum momento ele joga as rows para a variável, mas nao neste momento.
    var r;
    var cortes;
    //reseta o map.
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 10,
        center: { lat: 41.84173095, lng: -87.67467499},
        mapTypeId: google.maps.MapTypeId.TERRAIN
    });
    //Pega os dados.
    consumerColor
        .query()
        .withDataset('jcxq-k9xf')
        .select('ca,'+criterio)
        .order(criterio)
        .getRows()
        .on('success', function(rows){
            console.log("entrou na função"); 
            r = rows; 
            cortes = defCortes(r, criterio);
            //Agora ele acessa para pegar as coordenadas.
            consumer
                .query()
                .withDataset('igwz-8jzy')
                .getRows()
                .on('success', function (rows) {
                    for (var k = 0; k < rows.length; k++) {
                        var poligonosAreas = [];
                        var coords = rows[k].the_geom.coordinates[0][0];
                        var area_number = rows[k].area_numbe;   //Numero da área
                        for (var i = 0; i < coords.length; i++) {
                            var coord = coords[i];
                            poligonosAreas.push({ lat: coord[1], lng: coord[0] });
                        }
                        //Usa o numero da area para comparar com o que foi pego em r
                        //falta fazer
                        var area;
                        for (var i = r.length - 1; i >= 0; i--) {
                            if (r[i].ca == area_number){
                                area = r[i];
                                break;
                            }
                        }
                        var renda = parseFloat(area[criterio]);
                        var color = defColor(renda, cortes);
                        var poligonos = new google.maps.Polygon({
                            paths: poligonosAreas,
                            //strokeColor: '#FF0000',
                            strokeOpacity: 0.8,
                            strokeWeight: 1,
                            fillColor: color,
                            fillOpacity: 0.50,
                            map: map
                        });
                        //console.log(area.community_area_name + " / " + rows[k].community);
                        infoWindow = new google.maps.InfoWindow;

                    }
                }).on('error', function (error) { console.error(error); });

        });
}
function defCortes(valores, criterio){
    var maior = valores[valores.length-1][criterio];
    var menor = valores[0][criterio];
    var intervalo = (maior-menor)/5;
    var cortes = [];
    cortes[3] = maior - (intervalo);
    cortes[2] = maior - (intervalo*2);
    cortes[1] = maior - (intervalo*3);
    cortes[0] = maior - (intervalo*4);
    return cortes;
}

function defColor(value, cortes){
    if(value < cortes[0]){
        return 'LightBlue';
    }
    else if(value < cortes[1]){
        return 'DeepSkyBlue';
    }
    else if(value < cortes[2]){
        return 'Blue';
    }
    else{
        return 'MidnightBlue';
    }
}
