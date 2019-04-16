$(document).ready(function () {

    GenerateMap();

    function GenerateMap() {

        var map;
        var marker;

        DG.then(function () {
            map = DG.map('map', {
                center: [55.741216, 37.620921], // центр Москвы
                zoom: 11, // оптимальный зум для отображения всей Москвы при загрузке
                zoomControl: false,
                fullscreenControl: false
            });

            var markerIcon = DG.icon({
                iconUrl: '/images/marker.png',
                iconSize: [24, 32],
                iconAnchor: [12, 32]
            });


            // получение информации о координатах площадок
            $.get("/Map/GetMarkers", function (result) {

                for (i = 0; i < result.addresses.length; i++) {

                    var address = result.addresses[i];
                    // получение широты и долготы, так как передается строка
                    var separatorIndex = address.indexOf(',');
                    var objLatitude = address.substr(0, separatorIndex);
                    var objLongitude = address.substr(separatorIndex + 1);

                    marker = DG.marker([objLatitude, objLongitude],
                        {
                            icon: markerIcon
                        }
                    ).addTo(map);
                }

            });

        });

    }




});