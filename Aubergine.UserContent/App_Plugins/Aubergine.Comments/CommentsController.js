angular.module('umbraco').controller('ucCommentsController',
    function ($scope, $http, $routeParams, $filter, ucCommentsService) {

        $scope.loading = true;
        $scope.model.hideLabel = true;

        $scope.status = {
            Pending: 0,
            Spam: 1,
            Approved: 10
        };

        $scope.loadComments = function (id) {
            ucCommentsService.getComments(id)
                .then(function (response) {
                    $scope.comments = response.data;
                    $scope.loading = false;
                });
        }

        $scope.setStatus = function (key, status)
        {
            ucCommentsService.setStatus(key, status)
                .then(function (response) {
                    $scope.loadComments($routeParams.id)
                });
        };

        $scope.approve = function ($event, key) {
            $event.preventDefault();
            $scope.setStatus(key, $scope.status.Approved);
        }

        $scope.unapprove = function ($event, key) {
            $event.preventDefault();
            $scope.setStatus(key, $scope.status.Pending);
        }

        $scope.spam = function ($event, key) {
            $event.preventDefault();
            $scope.setStatus(key, $scope.status.Spam);
        }

        $scope.trash = function ($event, key) {
            $event.preventDefault();
            ucCommentsService.trash(key)
                .then(function (response) {
                    $scope.loadComments($routeParams.id)
                });
        }

        $scope.getProperty = function (comment, alias) {

            var value = $filter('filter')(comment.Properties, { PropertyAlias: alias }, true);
            if (value.length) {
                return value[0].Value;
            }
            return "";
        }

        $scope.loadComments($routeParams.id);

    });


// var serviceRoot = 'backoffice/CserContent/UserContentApi/';
