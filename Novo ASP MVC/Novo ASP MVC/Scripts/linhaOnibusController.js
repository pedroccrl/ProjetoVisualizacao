var app = angular.module('linhaOnibusApp', ['uiGmapgoogle-maps']);
app.controller('linhaOnibusController', function ($scope, $http, $interval) {
    $scope.map = { center: { latitude: -22.9191118, longitude: -43.3505867 }, zoom: 12 };
    $scope.urlApi = '/api/linhaonibus';
    $scope.linhas = [];

    $http.get($scope.urlApi).then(function (data) {
        for (var i = 0; i < data.data.length; i++) {
            var l = data.data[i];
            var path = [];
            for (var j = 0; j < l.Coordenadas.length; j++) {
                var c = l.Coordenadas[j];
                path.push({
                    latitude: c.Latitude,
                    longitude: c.Longitude
                });
                
            }
            $scope.linhas.push({
                Nome: l.Nome,
                Descricao: l.Descricao,
                stroke: {
                    color: '#FF0000',
                    weight: 3
                },
                path: path
            });
            
        }
    });




});

app.config(function (uiGmapGoogleMapApiProvider) {
    uiGmapGoogleMapApiProvider.configure({
        key: 'AIzaSyD9UfBAMHF2QhemIgsbX52OtYs8yslpsZ8',
        v: '3.20', //defaults to latest 3.X anyhow
        libraries: 'weather,geometry,visualization'
    });
})