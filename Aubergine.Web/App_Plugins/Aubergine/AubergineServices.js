angular.module('umbraco.resources').factory('aubergineService',
    function ($q, $http) {


        var serviceRoot = 'backoffice/Aubergine/AubergineDashboardApi/';

        return {

            getFeatures : function() {
                return $http.get(serviceRoot + 'GetFeatures');
            },

            getContent: function () {
                return $http.get(serviceRoot + 'GetContent');
            },

            installItem: function (id) {
                return $http.get(serviceRoot + 'installItem/' + id);
            }


        }

    });