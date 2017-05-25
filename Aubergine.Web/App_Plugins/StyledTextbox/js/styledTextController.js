function simpleStyledTextController($scope, localizationService) 
{
    $scope.limit = parseInt($scope.model.config.charCount);

    $scope.model.hideLabel = ($scope.model.config.hideLabel == 1);

    $scope.getClass = function () {
        if ($scope.limit > 0 && $scope.model.value != undefined)
        {
            if ($scope.model.value.length >= $scope.limit)
            {
                return 'text-danger';
            }
            return 'muted';
        }
    }

    $scope.limitMsg = localizationService.dictionary['styledtext_limit'] || 
        'you have reached the limit of {{limit}} characters';
    $scope.adviseMsg = localizationService.dictionary['styledtext_advice'] ||
        'you have gone over the advised length by {{over}} characters';

    $scope.remaining = localizationService.dictionary['styledtext_remaining'] ||
        '{{remain}} characters left';

    $scope.checkCharLimit = function () {
        if ($scope.limit > 0 && $scope.model.value != undefined) {
            if ($scope.model.value.length > $scope.limit) {

                if ($scope.model.config.enforceLimit == 1) {
                    $scope.model.value = $scope.model.value.substr(0, $scope.limit);
                    $scope.info = $scope.limitMsg.replace('{{limit}}', $scope.limit);
                }
                else {
                    $scope.info = $scope.adviseMsg.replace('{{over}}', ($scope.model.value.length -$scope.limit));
                }
            }
            else {
                $scope.info = $scope.remaining.replace('{{remain}}', ($scope.limit - $scope.model.value.length));
            }

        }
    }

    $scope.checkCharLimit();
}

angular.module('umbraco')
    .controller('jumoo.propEditor.styledTextController', simpleStyledTextController);