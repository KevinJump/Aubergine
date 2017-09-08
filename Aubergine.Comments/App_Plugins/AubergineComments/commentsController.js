(function() {

    'use strict';

    function commentsController($scope, $routeParams, $filter,
                                        notificationsService,
                                        aubergineCommentsService) 
    {

        $scope.model.hideLabel = true;

        var vm = this;

        vm.loaded = false; 
        vm.comments = [];
        vm.id = $routeParams.id;
        vm.statuses = { pending: 0, spam: 1, approved: 10 };

        vm.loadComments = LoadComments;
        vm.setStatus = SetStatus;
        vm.approve = Approve;
        vm.unApprove = UnApprove;
        vm.spam = Spam;
        vm.trash = Trash;

        vm.getProperty = GetProperty;

        refresh();

        /////////////

        function LoadComments(id) {
            aubergineCommentsService.getComments(id)
                .then(function (result) {
                    vm.comments = result.data; 
                    addActions(vm.comments);
                    vm.loaded = true;
                }, function (error) {
                    notificationsService.error("Load Failed", error.data.Message);
                });
        }

        function SetStatus(key, status) {
            aubergineCommentsService.setStatus(key, status, vm.id)
                .then(function (result) {
                    notificationsService.success("updated", "status updated");
                    refresh();
                }, function (error) {
                    notificationsService.error("Failed", error.data.Message);
                });
        }

        function Approve(key) {
            vm.setStatus(key, vm.statuses.approved);
        }

        function UnApprove(key) {
            vm.setStatus(key, vm.statuses.pending);
        }

        function Spam(key) {
            vm.setStatus(key, vm.statuses.spam);
        }

        function Trash(key) {
            aubergineCommentsService.trash(key)
                .then(function (result) {
                    notificationsService.success("trashed", "item deleted");
                    refresh();
                }, function (error) {
                    notificationsService.error("failed", error.data.Message);
                });
        }

        function GetProperty(comment, alias) {
            var value = $filter('filter')
                (comment.Properties, { PropertyAlias: alias }, true);
            if (value.length) {
                return value[0].Value;
            }
            return '';
        }

        ////////////////

        function refresh() {
            vm.loadComments(vm.id);
        }

        function addActions(comments) {
            comments.forEach(function (comment) {

                comment.buttons = {
                    defaultButton: {
                        labelKey: "aubComments_approve",
                        handler: function () { vm.approve(this.Key); },
                        Key: comment.Key
                    },
                    subButtons: [
                        {
                            labelKey: "aubComments_spam",
                            handler: function () { vm.spam(this.Key); },
                            Key: comment.Key
                        },
                        {
                            labelKey: "aubComments_trash",
                            handler: function () { vm.trash(this.Key); },
                            Key: comment.Key
                        }
                    ]
                }
                console.log('Status  : ' + comment.Status);
                console.log('Approved: ' + vm.statuses.approved);

                if (comment.Status == vm.statuses.approved) {
                    console.log('unapprove');
                    comment.buttons.defaultButton = {
                        labelKey: "aubComments_unapprove",
                        handler: function () { vm.unApprove(this.Key); },
                        Key: comment.Key
                    }
                }
            });
        }
    }

    angular.module('umbraco')
        .controller('aubergineCommentsController', commentsController);

})();
