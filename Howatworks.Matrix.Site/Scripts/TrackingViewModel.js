var TrackingViewModel = function (serviceUri, username) {

    var uri = serviceUri;

    var viewModel =
    {
        gameVersion: ko.observable("Live"),
        gameVersions: ko.observableArray(["Live"]),
        user: username,
        cmdrs: ko.observableArray([]),
        groups: ko.observableArray(["Default"]),
        group: ko.observable("undefined")


    };

    var lookupModeClass = function(cmdr) {
        if (cmdr == null) return "";
        if (cmdr.GameMode === "Solo") return "fa fa-lock";
        if (cmdr.GameMode === "Group") return "fa fa-unlock-alt";
        return "";
    };

    var lookupClass = function(cmdr) {
        if (cmdr == null) return "";
        if (cmdr.GameMode === "Solo") return "text-danger";
        if (cmdr.GameMode === "Group") return "text-warning";
        return "";
    };


    var gameModeContent = function(row) {
        if (row == null) return "";
        if (row.GameMode === "Solo") return "<i class='fa fa-lock' aria-hidden='true'></i>";
        if (row.GameMode === "Group") return "<i class='fa fa-unlock-alt' aria-hidden='true'></i> " + row.Group;
        return "";
    };

    var iconSnippet = function(className, bodyType, bodyName) {
        return "<i class='" + className + "' title='" + bodyType + "' aria-hidden='true'></i>" + bodyName;
    };

    var stackIconSnippet = function (className, modifierClassName, title, label) {
        return "<span class='fa-stack'><i class='" + className + " fa-stack-1x' title='" + title + "' aria-hidden='true'></i><i class='" + modifierClassName + " fa-stack-2x' aria-hidden='true'></i>" + label;
    };

    var bodyContent = function(row) {
        if (row == null) return "";
        if (row.Body == null) return "";
        if (row.Body.Type == null) return "";

        switch (row.Body.Type) {
        case "Station":
            if (row.Body.Docked) {
                return iconSnippet("fa fa-flag", row.Body.Type, row.Body.Name);
            } else {
                return iconSnippet("fa fa-flag-o", row.Body.Type, row.Body.Name);
            }
        case "Planet":
            return iconSnippet("fa fa-world", row.Body.Type, row.Body.Name);
        case "Null":
            if (cmdr.SignalSource === null) {
                return iconSnippet("fa", "", "");
            }
            return iconSnippet("fa fa-map-marker", "", "Signal Source");

        case "Star":
            return iconSnippet("fa fa-sun-o", row.Body.Type, row.Body.Name);
        case "PlanetaryRing":
            return iconSnippet("fa fa-dot-circle-o", row.Body.Type, row.Body.Name);
        default:
            return iconSnippet("fa", row.Body.Type, row.Body.Name);

        }

    };

    var lookupBodyDetail = function(cmdr) {
        return "";
    };

    var lookupShipStatus = function(row) {
        if (row == null) return "";

        var hullStatus = "";
        var hull = row.HullIntegrity;
        if (hull != null) {
            if (hull < 0.25) hullStatus = iconSnippet("fa fa-thermometer-quarter", "Critical", "");
            else if (hull < 0.50) hullStatus = iconSnippet("fa fa-thermometer-half", "Badly damaged", "");
            else if (hull < 0.75) hullStatus = iconSnippet("fa fa-thermometer-three-quarters", "Damaged", "");
            else if (hull < 1.00) hullStatus = iconSnippet("fa fa-thermometer-full", "Lightly damaged", "");
        }

        var shieldStatus = "";
        var shield = row.ShieldsUp;
        if (shield != null) {
            if (shield == true) {
                shieldStatus = stackIconSnippet("fa fa-shield", "fa fa-circle-thin", "Shields up", "");
            } else {
                shieldStatus = stackIconSnippet("fa fa-shield", "fa fa-ban text-danger", "Shields down", "");
            }
        }

        return hullStatus + shieldStatus;
    };

    var reloadGroups = function() {
        viewModel.groups.removeAll();
        $.getJSON(uri + viewModel.user + "/Groups", function (data) {
            for (var i = 0; i < data.length; i++) {
                viewModel.groups.push(data[i].Name);
            }

        });
    };

    var reloadVersions = function() {
        viewModel.gameVersions.removeAll();
        viewModel.gameVersions.push("Live");
        $.getJSON(uri + viewModel.user + "/Versions", function(data) {
            for (var i = 0; i < data.length; i++) {
                viewModel.gameVersions.push(data[i]);
            }
        });
    };

    var reloadCmdrs = function() {
        viewModel.cmdrs.removeAll();
        $.getJSON(uri + viewModel.group() + "/" + viewModel.gameVersion() + "/Tracking", function(data) {
            viewModel.cmdrs.removeAll();
            for (var i = 0; i < data.Tracking.length; i++) {
                var row = data.Tracking[i];
                var cmdr = {
                    rowclass: lookupClass(row),
                    cmdr: row.CommanderName,
                    gamemode: gameModeContent(row),
                    systemname: row.StarSystem.Name,
                    systemcoords: row.StarSystem.Coords[0] + ", " + row.StarSystem.Coords[1] + ", " + row.StarSystem.Coords[2],
                    body: bodyContent(row),
                    bodydetail: lookupBodyDetail(row),
                    ship: row.Ship,
                    shipstatus : lookupShipStatus(row)
                };
                viewModel.cmdrs.push(cmdr);
            }
        });
    };


    $(function() {


        viewModel.gameVersion.subscribe(function(newValue) {
            reloadCmdrs();
        });

        reloadVersions();
        reloadGroups();
        reloadCmdrs();

        var reloadTimer = setInterval(reloadCmdrs, 5000);

        ko.applyBindings(viewModel);

        viewModel.gameVersion("Live");

    });

};