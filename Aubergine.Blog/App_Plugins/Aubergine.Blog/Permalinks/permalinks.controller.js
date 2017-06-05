function permalinksController($scope) {
    $scope.options = [
        { name: "Numeric", sample: "http://sitename/blog/123/", value: "%post_id%/" },
        { name: "Day and Name", sample: "http://sitename/blog/2016/09/06/sample-post/", value: "%year%/%monthnum%/%day%/%postname%/" },
        { name: "Month and Name", sample: "http://sitename/blog/2016/09/sample-post/", value: "%year%/%monthnum%/%postname%/" },
        { name: "Post Name", sample: "http://sitename/blog/sample-post/", value: "%postname%/" },
    ];

    $scope.customPermalinkFormat = "";


    function Initialize() {
        if ($scope.model.value.charAt(0) == '-')
        {
            $scope.customPermalinkFormat = $scope.model.value.substring(1);
            $scope.model.value = '-';
        }
    }
    
    $scope.$on("formSubmitting", function (ev, args) {
        if ($scope.model.value == '-') {
            $scope.model.value = '-' + $scope.customPermalinkFormat;
        }
    });

    Initialize();
}


angular.module('umbraco')
    .controller('aubergine.permalinksController', permalinksController);