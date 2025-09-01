(function (usuarioDNP, idTipoProyecto, idTipoTramite) {
  controladorPowerBI.$inject = ["$scope"];

  function controladorPowerBI($scope) {
    var vm = this;
    vm.mesageError = null;
    vm.mesajeInicial = "Los datos de Power BI se generarán en este cuadro.";
    vm.models = window["powerbi-client"].models;
    vm.embedContainer;
    vm.$onInit = function () {
      cargarPowerBI();
      vm.conjuntoDeEventosFullScreen();
    };

    vm.conjuntoDeEventosFullScreen = conjuntoDeEventosFullScreen;

    function cargarPowerBI() {
      if (!vm.mesageError && vm.opciones  && validarOpciones(vm.opciones)) {
        renderizarInformeEnPantalla(vm.opciones);
      } else if(vm.mesageError && vm.opciones) {
        vm.mesageError = !vm.opciones ? "" : vm.opciones.ErrorMessage;
      }
    }

    

    $scope.$watch("vm.opciones", function () {
      cargarPowerBI();
    });

    function renderizarInformeEnPantalla(opciones) {
      try {
        vm.embedContainer = $("#embedContainer")[0];
          var filtro = vm.filtro;
          if (Array.isArray(filtro)) {
              var validados = [];

              angular.forEach(filtro, function (f) {
                  var valida = validarFiltro(f) ? f : null;
                  validados.push(valida);
              });

              filtro = validados;
          } else {
              filtro = validarFiltro(vm.filtro) ? [vm.filtro] : [null];
          }
        var config = {
          type: vm.type ? vm.type : 'report',
          tokenType: vm.models.TokenType.Embed,
          accessToken: opciones.EmbedToken.token,
          embedUrl: opciones.EmbedUrl,
          id: opciones.Id,
          permissions: vm.models.Permissions.All,
          filters: filtro,
          settings: {
            filterPaneEnabled: true,
            navContentPaneEnabled: true,
            layoutType: vm.models.LayoutType.Custom,
            localeSettings: {
              language: "es-ES",
              formatLocale: "es-ES",
            },
            hideErrors: true,
            customLayout: {
              // pageSize: {
              //   type: vm.models.PageSizeType.Widescreen,
              //   width: '100%',//1600,
              //   height: '100%',//1200,
              //  },
               displayOption: vm.models.DisplayOption.FitToWidth,
              // pagesLayout: {
              //   ReportSection1: {
              //     defaultLayout: {
              //       displayState: {
              //         mode: vm.models.VisualContainerDisplayMode.Hidden,
              //       },
              //     },
              //     visualsLayout: {
              //       VisualContainer1: {
              //         x: 1,
              //         y: 1,
              //         z: 1,
              //         width: 400,
              //         height: 300,
              //         displayState: {
              //           mode: vm.models.VisualContainerDisplayMode.Visible,
              //         },
              //       },
              //       VisualContainer2: {
              //         displayState: {
              //           mode: vm.models.VisualContainerDisplayMode.Visible,
              //         },
              //       },
                  // },
                // },
              // },
            },
          },
        };
        
        var report = powerbi.embed(vm.embedContainer, config);

        report.on("loaded", function () {
          console.log("report.on - loaded");
        });


        report.off("error");

        // Report.on will add an event handler that handles errors.
        // report.on("error", getErrors);
      } catch (ex) {
        vm.mesageError =
            "Algo sucedió al cargar los datos de Power BI. Por favor, póngase en contacto con el administrador del sistema. Mensaje: " + ex[0].message;
          console.error(ex[0].message);
      }
    }

    function getErrors(event) {
      const error = event.detail;

      // If error is not Fatal log the error and continue
      if (error.level !== models.TraceType.Fatal) {
        console.error(error);
        return;
      }

      // if the error is TokenExpired refresh the token
      // else, show error dialog
      if (error.message === models.CommonErrorCode.TokenExpired) {
        // refresh token
        // this function is not implemented as part of this solution
        // you can implement your own function here
        //var newAccessToken = refreshToken();

        // Get a reference to the embedded report.
        var report = powerbi.get(vm.embedContainer);

        // Set new access token
        //report.setAccessToken(newAccessToken);
      } else {
        // show error dialog with detailed message from error over the iframe
        // this function is not implemented as part of this solution
        // you can implement your own function here
        showError(error.detailedMessage);
      }
    }

    function validarOpciones(opciones) {
      if (!opciones) return false;
      if (!opciones.hasOwnProperty("Id")) return false;
      if (!opciones.hasOwnProperty("EmbedUrl")) return false;
      if (!opciones.hasOwnProperty("EmbedToken")) return false;
      if (!opciones.hasOwnProperty("EnableRLS")) return false;
      if (!opciones.hasOwnProperty("ErrorMessage")) return false;
      return true;
    }

    function validarFiltro(filtro) {
      if (!filtro) return false;
      if (!filtro.hasOwnProperty("$schema")) return false;
      if (!filtro.hasOwnProperty("target")) return false;
      if (!filtro.hasOwnProperty("operator")) return false;
      if (!filtro.hasOwnProperty("values")) return false;
      return true;
    }

  }

  function establecerClaseTotal(){
    if (document.fullscreenElement) {
      $('#fluidEmbed').addClass('fullscreen-powerbi');
      $('#embedContainer').addClass('fullscreen-powerbi');
      console.log('Element: ${document.fullscreenElement.id} entered full-screen mode.');
    } else {
      $('#fluidEmbed').removeClass('fullscreen-powerbi');
      $('#embedContainer').removeClass('fullscreen-powerbi');
      console.log('Leaving full-screen mode.');
    }
  }

  function conjuntoDeEventosFullScreen(){
    document.addEventListener("fullscreenchange", function() {
      establecerClaseTotal();
    });
    
    /* Firefox */
    document.addEventListener("mozfullscreenchange", function() {
      establecerClaseTotal();
    });
    
    /* Chrome, Safari and Opera */
    document.addEventListener("webkitfullscreenchange", function() {
      establecerClaseTotal();
    });
    
    /* IE / Edge */
    document.addEventListener("msfullscreenchange", function() {
      establecerClaseTotal();
    });
  }

  angular.module("backbone").component("powerbiEmbed", {
    templateUrl:
      "/src/app/comunes/componentes/powerBI/embedPowerBI.template.html",
    controller: controladorPowerBI,
    controllerAs: "vm",
    bindings: {
      opciones: "=",
      filtro: "=",
      type: "="
    },
  });
})(usuarioDNP, idTipoProyecto, idTipoTramite);
