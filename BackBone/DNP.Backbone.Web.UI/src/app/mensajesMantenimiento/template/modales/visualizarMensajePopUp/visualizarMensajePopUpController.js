(function() {

  visualizarMensajePopUpController.$inject = [
    "$scope",
    "$sce",
    "$uibModalInstance",
    "$location",
    'sesionServicios',
    "params"
  ];

  angular.module("backbone").controller("visualizarMensajePopUpController", visualizarMensajePopUpController);

  function visualizarMensajePopUpController(
    $scope,
    $sce,
    $uibModalInstance,
    $location,
    sesionServicios,
    params = required('params')
  ) {
    const vm = this;
    const required = (param) => new Error(`parameter ${param} is required`); 
    
    vm.iconos = {
      info: { 
        icon: "fa-info-circle", 
        style: { 'color': '#5a8dee', 'font-size': '30px' }
      },
      danger: {
        icon: "fa-times",
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

    vm.preVisualizacion = params.preVisualizacion;
    vm.restringeAcesso = params.restringeAcesso;
    vm.type = params.type || required('type');
    vm.template = params.template && $sce.trustAsHtml(params.template) || required('template');

    vm.init = function() {
      if(vm.restringeAcesso && !vm.preVisualizacion) {
        document.getElementsByClassName("modal")[0].classList.add("box-block")
        sesionServicios.eliminarTokens();
        setTimeout(() => {
          $uibModalInstance.dismiss('cerrar');
          _redirecionar()
        }, 10000)
      }
    }
    
    vm.cerrar = cerrar;

    function cerrar() {
      $uibModalInstance.dismiss('cerrar');
      if(vm.restringeAcesso && !vm.preVisualizacion)
        _redirecionar();
    }

    function _redirecionar(){
      $location.path("/Account/SignOut");
    }
  }
})();