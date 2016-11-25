var app = angular.module('brtApp', ['uiGmapgoogle-maps']);
app.controller('brtController', function ($scope, $http, $interval) {

    //this is for default map focus when load first time
    $scope.map = { center: { latitude: -22.9191118, longitude: -43.3505867 }, zoom: 12 }

    $scope.markers = [];
    $scope.locations = [];
    $scope.loading = "100";
    $scope.urlApi = '/api/brt';
    
    var att;
    att = $interval(function myfunction() {
        $http.get($scope.urlApi)
        .then(function (data) {

            //clear markers 
            $scope.markers = [];
            for (var i = 0; i < data.data.length; i++) {

                var v = data.data[i];
                $scope.markers.push({
                    Codigo: v.Codigo,
                    coords: { latitude: v.Latitude, longitude: v.Longitude },
                    Linha: v.Linha,
                    Velocidade: v.Velocidade,
                    //image: data.data.ImagePath
                });
            }

            //set map focus to center
            //$scope.map.center.latitude = data.data[0].Latitude;
            //$scope.map.center.longitude = data.data[0].Longitude;
            $scope.loading = "0";
        }, function () {
            alert('Error');
        });
    }, 10000);

    $scope.SelLinha = function (linha) {
        $scope.urlApi = '/api/brt/' + linha;
        $scope.loading = 100;
    }

    //Show / Hide marker on map
    $scope.windowOptions = {
        show: true
    };
    

});

app.config(function(uiGmapGoogleMapApiProvider) {
    uiGmapGoogleMapApiProvider.configure({
        key: 'AIzaSyD9UfBAMHF2QhemIgsbX52OtYs8yslpsZ8',
        v: '3.20', //defaults to latest 3.X anyhow
        libraries: 'weather,geometry,visualization'
    });
})

function atualiza(http) {

}