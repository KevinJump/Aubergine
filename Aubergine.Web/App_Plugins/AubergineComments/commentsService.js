/**
 *
 *
 *
 *
 *
 */

(function () {
    'use strict';

    function commentsService($http) {

        var serviceRoot = 'backoffice/Aubergine/UserContentApi/';
        var instance = "instance=Comments";

        var service = {
            getComments: GetComments,
            setStatus: SetStatus,
            trash: Trash
        };

        return service; 

        function GetComments(id) {
            return $http.get(serviceRoot + "GetUserContentByContentId/" + id + "?" + instance);
        }

        function SetStatus(key, status, pageId) {

            var data = {
                status: status,
                instance: "Comments",
                pageId: pageId
            };

            return $http.put(serviceRoot + "SetStatus/" + key, data);
        }

        function Trash(key) {
            return $http.delete(serviceRoot + "Trash/" + key + "?" + instance);
        }
    }

    angular.module('umbraco.resources')
        .factory('aubergineCommentsService', commentsService);
})();