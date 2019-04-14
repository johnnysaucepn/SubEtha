var LocationViewModel = function (serviceUri, username) {

    var uri = serviceUri;

    var viewModel =
    {
        gameVersion: ko.observable(""),
        gameVersions: ko.observableArray(["Live"]),
        user: username,
        starsystemname: ko.observable("Unknown"),
        starsystemcoords: ko.observable("0, 0, 0"),
        stationname: ko.observable("None"),
        stationtype: ko.observable(""),
        stationvisible: ko.observable(false)

    };

    var referencePoints = [
        { name: "Sagittarius A*", coords: { x: 25.21875, y: -20.90625, z: 25899.96875 } },
        { name: "Sol", coords: { x: 0, y: 0, z: 0 } },
        { name: "Colonia", coords: { x: -9530.5, y: -910.28125, z: 19808.125 } },
        { name: "Beagle Point", coords: { x: -1111.5625, y: -134.21875, z: 65269.75 } },
        { name: "Maia", coords: { x: -81.78125, y: -149.4375, z: -343.375 } }
    ];

    

    var reloadCurrentLocation = function(selectedVersion) {
        if (selectedVersion !== null) {

            $.getJSON(uri + viewModel.user + "/" + selectedVersion + "/Location",
                function(data) {
                    var location = data.Location;

                    viewModel.starsystemname(location.StarSystem.Name);
                    viewModel.starsystemcoords(location.StarSystem.Coords[0] +
                            ", " +
                            location.StarSystem.Coords[1] +
                            ", " +
                            location.StarSystem.Coords[2]),
                        viewModel.stationvisible(location.Station !== null);
                    viewModel.stationname(location.Station !== null ? location.Station.Name : "None");
                    viewModel.stationtype(location.Station !== null ? location.Station.Type : "");
                });
        }
    };

    var formatNameLabel = function(name, coords) {
        return name + " [" + coords[0] + ", " + coords[1] + ", " + coords[2] + "]";
    };

    var reloadLocationHistory = function(selectedVersion) {

        $.getJSON(uri + viewModel.user + "/" + selectedVersion + "/Systems/History",
            function(data) {
                var pointCount = data.Systems.length;

                var systems = [];
                var routes = [];

                var currentRoute = {
                    title: "Route",
                    points: []
                };
                for (var i = 0; i < pointCount; i++) {
                    var system = data.Systems[i].StarSystem;
                    if (system !== null) {
                        systems.push({
                            name: system.Name,
                            coords: {
                                x: system.Coords[0],
                                y: system.Coords[1],
                                z: system.Coords[2]
                            }
                        });
                        currentRoute.points.push({
                            s: system.Name
                        });
                    } else {
                        routes.push(currentRoute);
                        currentRoute = {
                            title: "Route",
                            points: []
                        };
                    }
                }
                routes.push(currentRoute);
                var content = { systems: systems, routes: routes };


                //Ed3d.init({
                //    basePath: "Content/ED3D/",
                //    container: 'graph',
                //    //camera: referencePoints[0].coords,
                //    json: content
                //});

                $("#graphcontent").html(JSON.stringify(content));

                Ed3d.rebuild();

            });


    };

    $(function () {

        $("#graphcontent").html(JSON.stringify(referencePoints));

        Ed3d.init({
            basePath: "Content/ED3D/",
            container: "graph",
            jsonContainer: "graphcontent"
            //camera: referencePoints[0].coords,
            //json: referencePoints
        });

        viewModel.gameVersions.removeAll();
        viewModel.gameVersions.push("Live");
        $.getJSON(uri + viewModel.user + "/Versions",
            function (data) {
                for (var i = 0; i < data.length; i++) {
                    viewModel.gameVersions.push(data[i]);
                }
            });

        viewModel.gameVersion.subscribe(function (newValue) {
            reloadCurrentLocation(newValue);
            reloadLocationHistory(newValue);
        });

        ko.applyBindings(viewModel);

        viewModel.gameVersion("Live");


    });
};