(function () {
    'use strict';
    

    function paginadorFormulario($scope) {
        var vm = this;
        $scope.pagina = {};
        $scope.pagingSize = $scope.pagingSize || 5;
        $scope.itemPorPagina = $scope.itemPorPagina || 10;

        function cargarPaginador(CantidadItem, actualPagina, itemPorPagina) {
            actualPagina = actualPagina || 1;
            var startPage, endPage;

            var totalPaginas = Math.ceil(CantidadItem / itemPorPagina);
            if (totalPaginas <= $scope.pagingSize) {
                startPage = 1;
                endPage = totalPaginas;
            } else {
                if (actualPagina + 1 >= totalPaginas) {
                    startPage = totalPaginas - ($scope.pagingSize - 1);
                    endPage = totalPaginas;
                } else {
                    startPage = actualPagina - parseInt($scope.pagingSize / 2);
                    startPage = startPage <= 0 ? 1 : startPage;
                    endPage = (startPage + $scope.pagingSize - 1) <= totalPaginas ? (startPage + $scope.pagingSize - 1) : totalPaginas;
                    if (totalPaginas === endPage) {
                        startPage = endPage - $scope.pagingSize + 1;
                    }
                }
            }

            var startIndex = (actualPagina - 1) * itemPorPagina;
            var endIndex = startIndex + itemPorPagina - 1;

            var index = startPage;
            var paginas = [];
            for (; index < endPage + 1; index++)
                paginas.push(index);

            $scope.pagina.actualPagina = actualPagina;
            $scope.pagina.totalPaginas = totalPaginas;
            $scope.pagina.startPage = startPage;
            $scope.pagina.endPage = endPage;
            $scope.pagina.startIndex = startIndex;
            $scope.pagina.endIndex = endIndex;
            $scope.pagina.paginas = paginas;
        }

        $scope.cargaPagina = function (actualPagina) {
            if (actualPagina < 1 || actualPagina > $scope.pagina.totalPaginas)
                return;

            if ($scope.totalItems === undefined)
                $scope.totalItems = [];

            cargarPaginador($scope.totalItems.length, actualPagina, $scope.itemPorPagina);
            $scope.displayItems = $scope.totalItems.slice($scope.pagina.startIndex, $scope.pagina.endIndex + 1);
            $scope.cambioDisplayItems = 1;
        };

        $scope.cargaPagina(1);
    }

    //function paginadorFormulario($scope) {
    //    $scope.pagingSize = 5;
    //    $scope.dataPerPage = 10;
    //    $scope.totalItems = [];

    //    //for (var i = 1; i <= 200; i++)
    //    //    $scope.totalItems.push(i);

    //    //$scope.displayItems = [];
    //}

   
    angular.module('backbone').component('paginadorFormulario', {
        controller: paginadorFormulario
    }).directive('pagingControl', [function () {
        return {
            restrict: 'E',
            templateUrl: "src/app/formulario/ventanas/comun/paginador/paginadorFormulario.html",
            controller: ['$scope', paginadorFormulario],
            scope: {
                totalItems: "=",
                displayItems: '=',
                pagingSize: '=',
                itemPorPagina: '=',
                cambioDisplayItems: '='
                // cargaPagina: '&callbackFn'
            },
            link: function (scope, element, attrs) {
               
            }
        };
    }]);
})();

    