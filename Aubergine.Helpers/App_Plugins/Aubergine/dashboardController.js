(function() {

    'use strict';

    function dashboardController($scope, aubergineServices,
                    notificationsService)
    {
        var vm = this;
        vm.viewState = 'list';
        vm.loaded = false; 
        vm.features = [];
        vm.content = [];

        vm.setViewState = SetViewState;
        vm.viewDetails = ViewDetails;
        vm.loadFeatures = LoadFeatures;
        vm.loadContent = LoadContent;
        vm.showDependencies = ShowDependencies;

        vm.install = Install;

        vm.loadFeatures();
        vm.loadContent();

        //////////
        function SetViewState(state) {
            vm.viewState = state;
        }

        function ViewDetails(group, type) {
            vm.viewState = 'detail';
            vm.group = group;
            vm.detailType = type; 
        }

        function LoadFeatures() {
            aubergineServices.getFeatures()
                .then(function (result) {
                    vm.features = result.data;
                }, function (error) {

                });
        }

        function LoadContent() {
            aubergineServices.getContent()
                .then(function (result) {
                    vm.content = result.data;
                });
        }

        function ShowDependencies(depends) {
            var items = vm.features;
            if (vm.detailType == 'content') {
                items = vm.content;
            }

            if (depends == undefined)
                return "";

            var guids = depends.split(',');
            var dependencies = [];

            guids.forEach(function (guid) {
                var packageId = guid.trim();
                var name = '[unknown]';
                items.forEach(function (item) {

                    if (item.Id == packageId) {
                        name = item.Name;
                    }
                });
                dependencies.push(name);
            });

            return dependencies.join(', ');
        }

        function Install(item, type) {
            console.log(item.Link);
        }

    }

    angular.module('umbraco')
        .controller('aubergineDashboardController', dashboardController);

})();