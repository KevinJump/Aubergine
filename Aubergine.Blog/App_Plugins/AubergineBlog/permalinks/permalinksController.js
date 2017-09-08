(function () {

    function permalinksController($scope) {

        var vm = this;

        var today = new Date();
        vm.year = today.getFullYear();
        vm.month = ('0' + (today.getMonth()+1)).slice(-2);
        vm.day = ('0' + today.getDate()).slice(-2);
           

        vm.options = [
            {
                name: "Numeric",
                sample: "http://sitename/blog/123",
                value: "%post_id/"
            },
            {
                name: "Day and Name",
                sample: "http://sitename/blog/" + vm.year + "/" + vm.month +"/" + vm.day + "/sample-post/",
                value: "%year%/%monthnum%/%day%/%postname%/"
            },
            {
                name: "Month and Name",
                sample: "http://sitename/blog/" + vm.year + "/" + vm.month + "/sample-post/",
                value: "%year%/%monthnum%/%postname%/"
            },
            {
                name: "Post Name",
                sample: "http://sitename/blog/sample-post/",
                value: "%postname%/"
            }
        ];

        vm.customFormat = '';

        $scope.$on("formSubmitting", function (event, args) {
            if ($scope.model.value == '-') {
                $scope.model.value = '-' + vm.customFormat;
            }
        });

        ///////////////

        function initialize() {
            if ($scope.model.value.charAt(0) == '-')
            {
                $scope.customFormat = $scope.model.value.substring(1);
                $scope.model.value = '-';
            }
        }

        initialize(); 
    }

    angular.module('umbraco')
        .controller('auberginePermalinksController', permalinksController);

})();