angular.module('backbone.core.directives')
    .directive('chonseKeyPress', function () {
        return {
            restrict: 'A',
            replace: false,
            scope: false,
            link: function ($scope, element, attrs) {
                const id = attrs.id;
                const selector = `#${id}_chosen > .chosen-drop > .chosen-search > input.chosen-search-input`;
                const input = document.querySelector(selector);
                if(attrs.ngChosenKeyUp && input)
                    input.addEventListener("keyup", $scope.vm[attrs.ngChosenKeyUp]);
            }
        };
    });