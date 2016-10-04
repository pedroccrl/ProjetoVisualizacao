var app = angular.module('brtApp', ['uiGmapgoogle-maps']);
app.controller('brtController', function ($scope, $http) {

    //this is for default map focus when load first time
    $scope.map = { center: { latitude: 22.590406, longitude: 88.366034 }, zoom: 16 }

    $scope.markers = [];
    $scope.locations = [];


    $http.get('/api/brt')
        .then(function (data) {
            //clear markers 
            $scope.markers = [];
            for (var i = 0; i < data.data.length; i++) {
    
                var v = data.data[i];
                $scope.markers.push({
                    id: v.Codigo,
                    coords: { latitude: v.Latitude, longitude: v.Longitude },
                    title: v.Linha,
                    vel: data.data.Velocidade,
                    //image: data.data.ImagePath
                });
            }
            console.log($scope.markers);

            //set map focus to center
            $scope.map.center.latitude = data.data[0].Latitude;
            $scope.map.center.longitude = data.data[0].Longitude;

    }, function () {
        alert('Error');
    });


    $scope.ShowLocation = function (Codigo) {
        alert('Oi');
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