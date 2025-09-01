(function() {

    visualizarMensajeDisclaimer.$inject = ["$scope", "$sce", "$location", "sesionServicios"];
    function visualizarMensajeDisclaimer($scope, $sce, $location, sesionServicios) {
      const vm = this;
      
      vm.timeouts = [];
      vm.iconos = {
        warning: { 
            icon: "fa-exclamation-triangle",
            alert: "alert-warning", 
            style: { 'color': 'white', 'font-size': '20px' },
        },
        danger: {
          icon: "fa-times",
          alert: "alert-danger",
          style: {
            'color': 'white',
            'font-size': '20px',
            'background': 'rgb(244, 67, 54)',
            'width': '30px',
            'height': '30px',
            'border-radius': '50%',
            'text-align': 'center',
            'line-height': '0px',
            'vertical-align': 'middle',
            'padding-top': '15px',
            'padding-left': '2px' 
          }
        }
      }
      
      function renderizarTemplate(template) {
        return template && $sce.trustAsHtml(template);
      }

      function cerrar() {
        vm.show = false;
        if(vm.restringeAcesso)
          _redirecionar();
      }

      function _redirecionar(){
        $location.path("/Account/SignOut");
      }

      $scope.$watch("vm.show", function (value) {
          if(value)
            vm.init();
      });

      vm.init = function() {
        if(vm.restringeAcesso && !vm.preVisualizacion) {
          sesionServicios.eliminarTokens();
          setTimeout(() => vm.cerrar(), 8000);
          return;
        }

        if(vm.preVisualizacion) {
          vm.timeouts.forEach(time => clearTimeout(time));
          vm.timeouts.push(setTimeout(() => {
              vm.show = false;
              $('.pre .alert-warning, .pre .alert-danger').fadeOut(2000);
          }, 8000)); 
        }
      }
  
      vm.cerrar = cerrar;
      vm.renderizarTemplate = renderizarTemplate;
    }

    angular.module('backbone')
        .component('mesajeDisclaimer', {
            templateUrl: "/src/app/mensajesMantenimiento/componentes/visualizarMensajeDisclaimer/visualizarMensajeDisclaimer.html",
            controller: visualizarMensajeDisclaimer,
            controllerAs: 'vm',
            bindings: {
                type: "=",
                template: "=",
                show: "=",
                preVisualizacion: "=",
                restringeAcesso: "=",
            }
        });
  })();