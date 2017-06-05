angular.module('umbraco.resources').factory('ucCommentsService',
    function ($q, $http) {

        var serviceRoot = 'backoffice/Aubergine/UserContentApi/';

        return {
            getComments: function (id) {
                return $http.get(serviceRoot + "GetUserContent/" + id + "?instance=comments"); // + "?type=comment");
            },

            setStatus: function (key, status) {
                return $http.get(serviceRoot + "SetStatus/?id=" + key + "&status=" + status + "&instance=comments");
            },

            trash: function (key) {
                return $http.get(serviceRoot + "trash/" + key + "&instance=comments");
            }

        }
    });