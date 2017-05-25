angular.module("umbraco").controller("Our.Umbraco.Switcher.Controller", function ($scope, $timeout, angularHelper, localizationService) {

    $scope.switchStyle = ($scope.model.config.switchClass != undefined || $scope.model.config.switchClass == "") ? $scope.model.config.switchClass : "";
    $scope.showLabel = $scope.model.config.hideLabel == false || $scope.model.config.hideLabel == undefined;
    $scope.statusLeftRight = $scope.model.config.statusLeftRight == true;

    $scope.showIcons = $scope.model.config.showIcons && $scope.model.config.showIcons !== '0' ? true : false;

    var alreadyDirty = false;

    if($scope.model.config.onLabelText === null || $scope.model.config.onLabelText === "") {
        $scope.onLabelText = "On";
    } else {
        $scope.onLabelText = $scope.model.config.onLabelText;

        var re = new RegExp(/^(?:\{\{)(?:[^\ ]*)(?:\}\})$/gm);
        var str = $scope.onLabelText;
        //console.log(str);
        
        if (re.test(str)) {
            // the string contains match the pattern "{{ }}"

            str = str.trim().replace("{{","").replace("}}","");

            localizationService.localize(str)
            .always(function(response) {
                $scope.offLabelText = "[" + str + "]";
            })
            .then(function (value) {
                var text = value != null ? value : "";
                //console.log(text);
                $scope.onLabelText = text;
            });
        }
    }

    if($scope.model.config.offLabelText === null || $scope.model.config.offLabelText === "") {
        $scope.offLabelText = "Off";
    } else {
        $scope.offLabelText = $scope.model.config.offLabelText;

        var re = new RegExp(/^(?:\{\{)(?:[^\ ]*)(?:\}\})$/gm);
        var str = $scope.offLabelText;

        if (re.test(str)) {
            // the string match the pattern "{{ }}"

            str = str.trim().replace("{{","").replace("}}","");
            
            localizationService.localize(str)
            .always(function(response) {
                $scope.offLabelText = "[" + str + "]";
            })
            .then(function (value) {
                var text = value != null ? value : "";
                $scope.offLabelText = text;
            });
        }
    }

    $scope.model.textLeft = "";
    $scope.model.textRight = "";

    if ($scope.model.value === null || $scope.model.value === "") {
        $scope.enabled = $scope.model.config.switchOn == true;
    } else {
        $scope.enabled = $scope.model.value == 1;
    }

    $scope.$watch('enabled', function (newval, oldval) {
        //console.log(newval, oldval);
        $scope.model.value = newval === true ? 1 : 0;

        if ($scope.model.value == 1) {
            $scope.model.textRight = $scope.onLabelText;

            if ($scope.statusLeftRight) {
                $scope.model.textLeft = $scope.offLabelText;
            }
        }
        else {
            $scope.model.textRight = $scope.offLabelText;

            if ($scope.statusLeftRight) {
                $scope.model.textLeft = $scope.offLabelText;
                $scope.model.textRight = $scope.onLabelText;
            }
        }

        if(newval !== oldval) {
            //run after DOM is loaded
            $timeout(function () {
                if (!alreadyDirty) {
                    var currForm = angularHelper.getCurrentForm($scope);
                    currForm.$setDirty();
                    alreadyDirty = true;
                }
            }, 0);
        }

    }, true);

});