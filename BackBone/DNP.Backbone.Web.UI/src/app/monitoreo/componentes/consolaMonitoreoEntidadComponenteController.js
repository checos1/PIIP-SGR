(function () {
  "use strict";
  angular
    .module("backbone")
    .controller(
      "consolaMonitoreoEntidadComponenteController",
      consolaMonitoreoEntidadComponenteController
    );

  consolaMonitoreoEntidadComponenteController.$inject = [
    "$scope",
    "$filter",
    "sesionServicios",
    "backboneServicios",
    "proyectoServicio",
    "uiGridConstants",
    "servicioConsolaMonitoreo"
  ];

  function consolaMonitoreoEntidadComponenteController(
    $scope,
    $filter,
    sesionServicios,
    backboneServicios,
    proyectoServicio,
    uiGridConstants,
    servicioConsolaMonitoreo
  ) {
    var vm = this;

    //#region  variables

    vm.gridOptions;
    vm.Mensaje = "";
    vm.entidadSubGridTemplate =
      "src/app/monitoreo/plantillas/plantillaEntidadSubGrid.html";
    vm.entidadTemplate = "src/app/monitoreo/plantillas/plantillaEntidad.html";
    vm.peticion = obtenerPeticion();
    vm.embedType = 'report';
    vm.fulscreen= fulscreen;
    vm.embedFiltro = {
      $schema: "http://powerbi.com/product/schema#basic",
      target: {
        table: "Entidades",
        column: "EntidadId",
      },
      operator: "In",
      values: [],
      displaySettings: {
        isLockedInViewMode: true,
        isHiddenInViewMode: false,
        displayName: "Filtro por ID Entidad",
      },
      };

    vm.toogleAmpliar = false;

    vm.grupoEntidadesVacios = [
      { tipoEntidad: "Nacional" },
      { tipoEntidad: "Territorial" },
      { tipoEntidad: "SGR" },
      { tipoEntidad: "Privadas" },
      { tipoEntidad: "Públicas" },
    ];

    function fulscreen(element){      
      var el = document.getElementById('element');       
      el.requestFullscreen();
      }
    

    function obtenerPeticion() {
      var roles = sesionServicios.obtenerUsuarioIdsRoles();
      if (
        backboneServicios.estaAutorizado() &&
        roles != null &&
        roles.length > 0
      )
        return {
          IdUsuario: usuarioDNP,
          Aplicacion: nombreAplicacionBackbone,
          IdObjeto: "bc154cba-50a5-4209-81ce-7c0ff0aec2ce",
          ListaIdsRoles: roles,
        };

      return {};
    }

    vm.consultarAccionPowerBI = function (idEntidad) {
      var filtro = {
          ReportId: "4923af10-8426-483b-944f-082f4e50cdb6"
      };
      servicioConsolaMonitoreo
        .obtenerConsolaMonitoreoReportes(vm.peticion,filtro)
        .then(
          function (retorno) {
            vm.embedFiltro.values = [parseInt(idEntidad)];
            vm.embedConfig = retorno.data;
          },
          function (error) {
            vm.Mensaje = error.data.Message;
            mostrarMensajeRespuesta();
          }
        );
    };

    vm.columnDefPrincial = [
      {
        field: "tipoEntidad",
        displayName: "Grupo",
        enableHiding: false,
        width: "100%",
        cellTemplate: vm.grupoEntidadTemplate,
      },
    ];

    vm.columnDef = [
      {
        field: "entidad",
        displayName: "Entidade",
        width: "100%",
        enableHiding: false,
        cellTemplate: vm.entidadTemplate,
      },
    ];

    function onRegisterApi(gridApi) {
      $scope.gridApi = gridApi;
    }

    this.$onInit = function () {

      if (!vm.gridOptions) {
        vm.gridOptions = {
          expandableRowTemplate: vm.entidadSubGridTemplate,
          expandableRowScope: {
            subGridVariable: "subGridScopeVariable",
          },
          expandableRowHeight: 200,
          enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
          enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
          enableMinHeightCheck: true,
          enableColumnResizing: false,
          showGridFooter: false,
          enablePaginationControls: false,
          useExternalPagination: false,
          useExternalSorting: false,
          paginationCurrentPage: 1,
          enableVerticalScrollbar: 1,
          enableFiltering: false,
          showHeader: false,
          useExternalFiltering: false,
          onRegisterApi: onRegisterApi,
        };

        vm.gridOptions.columnDefs = vm.columnDefPrincial;
        vm.gridOptions.data = [];
      }

      var roles = sesionServicios.obtenerUsuarioIdsRoles();
      if (
        backboneServicios.estaAutorizado() &&
        roles != null &&
        roles.length > 0
      )
        vm.cargarEntidades();
    };

    vm.cargarEntidades = function () {
      const exito = function (respuesta) {
        if (
          respuesta.data.GruposEntidades &&
          respuesta.data.GruposEntidades.length == 0
        )
          return;
          
        vm.listaGrupoEntidades = respuesta.data.GruposEntidades.reduce(
          (acc, grupoEntidad) => {
            acc.push({
              tipoEntidad: grupoEntidad.TipoEntidad,
              subGridOptions: {
                appScopeProvider: $scope,
                paginationPageSizes: [5],
                paginationPageSize: 5,
                columnDefs: vm.columnDef,
                enablePaginationControls: false,
                showHeader: false,
                data: grupoEntidad.ListaEntidades.map((x) => ({
                  entidad: x.NombreEntidad,
                  entidadId: x.IdEntidad,
                })),
              },
            });

            return acc;
          },
          []
        );

        const vacios = vm.grupoEntidadesVacios.filter(x =>
            !vm.listaGrupoEntidades.find(y => y.tipoEntidad === x.tipoEntidad)
        );
        const result = vm.listaGrupoEntidades.concat(vacios);
        vm.gridOptions.data = result;
        vm.Mensaje = respuesta.data.Mensaje;
      };

      function error(error) {
        const httpStatus = {
          401: () => $filter("language")("ErrorUsuarioSinPermisosAplicacion"),
          500: () => $filter("language")("ErrorObtenerDatos"),
        };

        if (error && !error.status) return;

        const handle = httpStatus[error.status];
        vm.Mensaje = (handle && handle()) || error.statusText;
      }

      return proyectoServicio.obtenerProyectos(vm.peticion).then(exito, error);
      };

    vm.toogleFullScreen = function() {
          vm.toogleAmpliar = !vm.toogleAmpliar;
          lista();
      }

    //#endregion
  }

  angular.module("backbone").component("monitoreoEntidad", {
    templateUrl:
      "/src/app/monitoreo/componentes/consolaMonitoreoEntidadComponente.html",
    controller: "consolaMonitoreoEntidadComponenteController",
    controllerAs: "vm",
  });
})();
