angular.module('umbraco').controller('aubergineDashboardController',
    function ($scope, aubergineService) {

        var vm = this;

        vm.viewState = 'list';
        vm.featuresLoaded = false;
        vm.contentLoaded = false; 

        vm.setViewState = function (state) {
            vm.viewState = state;
        }

        vm.viewDetails = function (group, type) {
            vm.viewState = 'details';
            vm.group = group;
            vm.detailType = type;
        }


        vm.loadFeatures = function (type) {
            aubergineService.getFeatures()
                .then(function (response) {
                    vm.features = response.data;
                    vm.featuresLoaded = true;
                });
        }

        vm.loadContent = function (type) {
            aubergineService.getContent()
                .then(function (response) {
                    vm.content = response.data;
                    vm.contentLoaded = true;
                });
        }

        vm.showDependencies = function (depends) {

            var items = vm.features;
            if (vm.detailType == 'content') {
                items = vm.content;
            }

            if (depends === undefined)
                return "";

            var guids = depends.split(',');
            var dependencies = [];

            for (var i = 0; i < guids.length; i++) {
                var packageId = guids[i].trim();
                var packageName = '[unknown]'
                for (var j = 0; j < items.length; j++) {
                    if (items[j].Id == packageId) {
                        console.log(items[j].Name);
                        packageName = items[j].Name;
                    }
                }
                dependencies.push(packageName);
            }

            return dependencies.join(', ');
        }

        vm.loadFeatures();
        vm.loadContent();

    });