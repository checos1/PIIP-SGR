(function () {
    mensajesMantenimientoController.$inject = [
      '$scope',
      'MensajeMantenimientoModel',
      'mensajeServicio',
      'sesionServicios',
      'array.extensions'
    ];

    function mensajesMantenimientoController(
      $scope,
      MensajeMantenimientoModel,
      mensajeServicio,
      sesionServicios
    ) {
      const vm = this;

      vm.disclaimers = [];
      vm.modals = [];
      vm.timeouts = [];
      vm.mensajes = [];

      vm.$onInit = function () { };

        $scope.$on('MensajesMantenimientoConfirmada', function (event, data) {
            vm.mensajes = vm.mensajes || [];
            let mensajesTemp = [];
            const idsMesajes = vm.mensajes.map(m => m.Id);
            const respuesta = (Array.isArray(data) && data || []).map(x => new MensajeMantenimientoModel(x));
            const nuevasMensajes = respuesta.filter(x => !idsMesajes.includes(x.Id));
            const mensajesActualizadas = respuesta.filter(x => vm.mensajes.some(m => m.Id == x.Id && m.comparar(x)));
            if (!nuevasMensajes.length && !mensajesActualizadas.length)
                return;

            const entidades = sesionServicios.obtenerUsuarioEntidades();
            const mensajesNacional = nuevasMensajes.filter(x => x.TipoEntidad == 'Nacional');
            if(mensajesNacional.length && !entidades.some(x => x.TipoEntidad == 'Nacional'))
              mensajesTemp = vm.mensajes.concat(nuevasMensajes.filter(x => x.TipoEntidad != 'Nacional'));
            else
              mensajesTemp = vm.mensajes.concat(nuevasMensajes);
        
            if(vm.mensajes.length == mensajesTemp.length && !mensajesActualizadas.length)
              return;

            if(mensajesTemp.some(x => !idsMesajes.includes(x.Id)) || !vm.mensajes.length)
              vm.mensajes = vm.mensajes.concat(mensajesTemp.filter(x => !idsMesajes.includes(x.Id)));
            
            vm.mensajes = (mensajesActualizadas || []).reduce((acc, mensajeActualiza) => {
              let mensajeAntiguo = acc.find(x => x.Id == mensajeActualiza.Id);
              if(mensajeAntiguo) {
                acc.remove(mensajeAntiguo);
                acc.push(mensajeActualiza);
              }

              return acc;
            }, vm.mensajes);

            vm.disclaimers = vm.mensajes.filter(x => x.TipoMensaje == 2)
              .sort(x => x.RestringeAcesso && 1 || -1)
              .map(x => (
                {
                  template: x.MensajeTemplate,
                  type: x.EstiloTipoMensaje,
                  restringeAcesso: x.RestringeAcesso,
                  show: true
                }
              ));

            vm.modals = vm.mensajes.filter(x => x.TipoMensaje == 1)
              .sort(x => x.RestringeAcesso && 1 || -1)
              .map(x => mensajeServicio.visualizarModalMensaje(x));
      });
    }
  
    angular.module("backbone").component("mensajesMantenimiento", {
      templateUrl: "src/app/comunes/componentes/mensajesMantenimiento/mensajesMantenimiento.template.html",
      controller: mensajesMantenimientoController,
      controllerAs: "vm"
    });
})();