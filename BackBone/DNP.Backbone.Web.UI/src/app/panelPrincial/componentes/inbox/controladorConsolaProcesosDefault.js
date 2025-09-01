(function () {
    'use strict';
    angular.module('backbone').controller('consolaprocesosdefaultController', consolaprocesosdefaultController);
    consolaprocesosdefaultController.$inject = [
        '$scope'
    ];
    function consolaprocesosdefaultController(
        $scope
    ) {
        var vm = this;
        vm.Init = function () {
        }
    }

    angular.module('backbone')
        .component('consolaprocesosdefault', {
            templateUrl: "/src/app/panelPrincial/componentes/inbox/consolaprocesosdefault.template.html",
            controller: 'consolaprocesosdefaultController',
            controllerAs: 'vm'
        });
})();