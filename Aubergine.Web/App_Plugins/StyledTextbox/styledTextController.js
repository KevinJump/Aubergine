/**
 * @ngdoc controller
 *
 * @name aubergine.styledTextboxController
 *
 * @function
 *
 * @description
 *   handles the styled textbox property editor
 *   mainly just for limiting the box, and displaying any 
 *   messages when users reach / exceed the limits.
 */
(function () {

    'use strict';

    function styledTextController($scope, localizationService) {

        $scope.model.hideLabel = ($scope.model.config.hideLabel === "1");

        var vm = this;

        vm.change = change;

        vm.config = $scope.model.config;
        vm.limit = vm.config.charCount * 1;
        vm.enforce = vm.config.enforceLimit * 1;

        vm.message = '';
        vm.messageClass = 'muted';
        vm.remainingMsg = 'You have reached the limit of %limit% characters';
        vm.adviceMsg = 'Over by %over% charecters';
        vm.limitMsg = '%remain% characters left';

        localizationService.localize("styledtext_limit").then(function (value) { vm.limitMsg = value });
        localizationService.localize("styledtext_advice").then(function (value) { vm.adviceMsg = value });
        localizationService.localize("styledtext_remaining").then(function (value) {
            vm.remainingMsg = value;
            vm.change();
        });
                

        ///////////////////////////////////
        function change() {
            if (vm.limit > 0 && $scope.model.value != undefined) {
                if ($scope.model.value.length > vm.limit) {
                    vm.messageClass = 'text-danger';

                    if (vm.enforce === 1) {
                        vm.message = vm.limitMsg.replace('%limit%', vm.limit);
                        $scope.model.value = $scope.model.value.substr(0, vm.limit);
                    }
                    else {
                        vm.message = vm.adviceMsg.replace('%over%', $scope.model.value.length - vm.limit);
                    }
                }
                else {
                    vm.messageClass = 'muted';
                    vm.message = vm.remainingMsg.replace('%remain%', vm.limit - $scope.model.value.length);
                }
            }
        }
    }

    angular.module('umbraco')
        .controller('aubergine.styledTextController', styledTextController);

})();