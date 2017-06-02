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

        vm.loadFeatures();
        vm.loadContent();

    });