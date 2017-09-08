(function () {
    'use strict';

    function aubergineServices($http) {

        var serviceRoot = 'backoffice/Aubergine/AubergineDashboardApi/';

        var service = {
            getFeatures: GetFeatures,
            getContent: GetContent,
            InstallItem: InstallItem
        };

        return service;

        //////////////

        function GetFeatures() {
            return $http.get(serviceRoot + "GetFeatures");
        }

        function GetContent() {
            return $http.get(serviceRoot + "GetContent");
        }

        function InstallItem() {
            return $http.get(serviceRoot + "InstallItem/" + id);
        }

    }

    angular.module('umbraco.resources')
        .factory('aubergineServices', aubergineServices);

})();