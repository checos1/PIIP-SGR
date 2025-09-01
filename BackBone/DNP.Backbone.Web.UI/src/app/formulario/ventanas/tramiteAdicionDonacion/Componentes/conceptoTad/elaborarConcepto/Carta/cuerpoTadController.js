(function () {
    'use strict';

    cuerpoTadController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'cartaServicio',
        'servicioAcciones',
        'justificacionCambiosServicio',
        'datosAdicionDonacionServicio'
    ];

    function cuerpoTadController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        cartaServicio,
        servicioAcciones,
        justificacionCambiosServicio,
        datosAdicionDonacionServicio
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.nombreSccion = 'Cuerpo';
        vm.conceptoValor = 'Concepto favorable';
        //vm.tipoTramite = 1;
        vm.tipoTramite = vm.tipotramiteid;
        vm.tramiteId = vm.tramiteid;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.listadoAccionesTramite[0].Entidad;

        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY");
        vm.numeroRadicado = 0;
        obtenerFechaSinHoras(new Date());
        vm.totalContra = 0;
        vm.totalCred = 0;
        vm.tipoRecurso = "";
        vm.RecursoNacion = false;
        vm.RecursoPropios = false;

        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.mostrarConstantes = false;
        vm.heigprograma = 150;
        vm.mostrarCDP = true;
        vm.mostrarCRP = false;
        vm.mostrarSeccionCDP = true;

        /*variables objetos*/

        vm.plantillaCarta = {};
        vm.carta = {};
        vm.listaCampos = [];


        /*variables configuación grids */

        vm.gridCDPGR = {};
        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }
        vm.columnDefCDPRP = [
            {
                field: 'cdp',
                displayName: 'Número CDP ',
                enableHiding: false,
                minWidth: 200,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.cdp}} </label></div > '
            },
            {
                field: 'fechaCDP',
                displayName: 'Fecha CDP',
                cellClass: 'col-numcorto',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.fechaCDP}} </label></div > '
            },
            {
                field: 'valorCDP',
                displayName: 'Valor CDP Trámite',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div class="text-left"><label style="font-weight: 400; font-size: 16px; font-style: normal;">{{row.entity.valorCDP | currency}} </label></div > ',
                cellClass: 'col-valor'

            },
        ];

        vm.datosCDPGR = [];
        vm.datosAutCredito = {};
        vm.datosAutContraCredito = {};

        vm.proyectosContraCredito = [];
        vm.proyectosCredito = [];
        vm.programaContraCredito = [];
        vm.programaCredito = [];
        vm.subprogramaContraCredito = [];
        vm.subprogramaCredito = [];

        /*Carga grids */

        vm.gridCDPGR = {
            enableSorting: true,
            enableRowSelection: false,
            enableFullRowSelection: false,
            multiSelect: false,
            enableRowHeaderSelection: false,
            enableColumnMenus: false,
            columnDefs: vm.columnDefCDPRP,
            data: vm.datosCDPGR
        };

        vm.gridCDPGR.appScopeProvider = vm;
        vm.gridCDPGR.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
        };

        /* Metodos **/
        vm.guardarCuerpoConcepto = guardarCuerpoConcepto;

        $scope.$watch('vm.rolanalista', function () {
            if (vm.novalidarconcepto) {
                vm.habilitaBotones = true;
            }
            else {
                if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                    vm.habilitaBotones = vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false;
                }
            }
        });

        $scope.$watch('vm.deshabilitar', function () {
            if (vm.novalidarconcepto) {
                vm.habilitaBotones = true;
            }
            else {
                if (vm.deshabilitar === "true") {
                    vm.habilitaBotones = false && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
                }
                else if (vm.deshabilitar === "false") {
                    vm.habilitaBotones = true && vm.rolanalista.toLowerCase() === 'true' && !$sessionStorage.soloLectura ? true : false; false;
                }
            }
        });

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });


        function init() {
            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        vm.disabled = true;
                        vm.tipoTramite = vm.tipotramiteid;
                        vm.tramiteId = vm.tramiteid;
                        vm.abrirpanel();

                        consultarAutorizacion();
                        obtenerPlantillaCarta();
                        
                        vm.mostrarSeccionCDP = false;
                        consultarConvenio();
                        
                    }
                }, true);


        }

        vm.abrirpanel = function () {
            var panel = document.getElementById('panelcuerpoconcepto');;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = "3500px";

            }
        }

        function obtenerPlantillaCarta() {
            cartaServicio.obtenerPlantillaCarta(vm.nombreSccion, vm.tipoTramite)
                .then(resultado => {
                    if (resultado != undefined && resultado.data) {
                        var PlantillaSecciones = [];
                        vm.listaCampos = [];
                        if (resultado.data.PlantillaSecciones != undefined) {
                            var seccion = {};
                            resultado.data.PlantillaSecciones.map(function (item) {
                                seccion.Id = item.Id;
                                seccion.PlantillaCartaId = item.PlantillaCartaId;
                                seccion.PlantillaSeccionCampos = [];
                                item.PlantillaSeccionCampos.map(function (itemCampo) {
                                    var campo = {};
                                    campo.Id = itemCampo.Id;
                                    campo.PlantillaCartaSeccionId = itemCampo.PlantillaCartaSeccionId;
                                    campo.NombreCampo = itemCampo.NombreCampo;
                                    campo.TipoCampo = itemCampo.TipoCampo;
                                    campo.TituloCampo = itemCampo.TituloCampo;
                                    campo.TextoDefecto = itemCampo.TextoDefecto;
                                    campo.Editable = itemCampo.Editable;
                                    campo.Orden = itemCampo.Orden;
                                    seccion.PlantillaSeccionCampos.push(campo);
                                    vm.listaCampos.push(campo);

                                });
                            });
                            PlantillaSecciones.push(seccion);
                        }


                        vm.plantillaCarta.Id = resultado.data.Id;
                        vm.plantillaCarta.IipoTramiteId = resultado.data.IipoTramiteId;
                        vm.plantillaCarta.PlantillaSecciones = PlantillaSecciones;
                        obtenerDatosIniciales();
                    }
                    else {
                        vm.plantillaCarta = {};
                    }

                });
        }

        function obtenerDatosIniciales() {
            var idPlantillaSeccion = vm.plantillaCarta.PlantillaSecciones[0].Id;
            cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, idPlantillaSeccion)
                .then(resultado => {
                    if (resultado != undefined && resultado.data != null && resultado.data.length > 0) {
                        var tmp = resultado.data[0];
                        vm.carta.Id = tmp.Id;
                        vm.carta.TramiteId = tmp.TramiteId;
                        vm.carta.Proceso = tmp.Proceso;
                        vm.carta.Entidad = tmp.Entidad;
                        vm.carta.TipoId = vm.tipoTramite;

                        var cartaSeccion = [];
                        if (tmp.ListaCartaSecciones != undefined && tmp.ListaCartaSecciones.length > 0) {
                            var plantillaseccion = {};
                            tmp.ListaCartaSecciones.map(function (item) {
                                plantillaseccion = {
                                    Id: item.Id,
                                    CartaId: item.CartaId,
                                    ListaCartaCampos: vm.listaCampos.map(function (iteml) {
                                        var campo = {};
                                        var campotmp = item.ListaCartaCampos.find(x => x.NombreCampo === iteml.NombreCampo);
                                        if (campotmp) {
                                            campo.Id = campotmp.Id;
                                            //campo.Orden = campotmp.Orden;
                                            campo.CartaConceptoSeccionId = campotmp.CartaConceptoSeccionId;
                                            campo.PlantillaCartaCampoId = campotmp.PlantillaCartaCampoId;
                                            campo.DatoValor = campotmp.DatoValor !== undefined && campotmp.DatoValor !== '' ? campotmp.DatoValor : iteml.TextoDefecto;
                                            campo.NombreCampo = campotmp.NombreCampo;
                                            campo.Longitud = campotmp.TipoCampo.Longitud;
                                            campo.Tipo = campotmp.TipoCampo.Tipo;
                                            campo.ValorDefecto = '';
                                        }
                                        else {
                                            campo.Id = iteml.Id;
                                            //campo.Orden = campotmp.Orden;
                                            campo.CartaConceptoSeccionId = iteml.CartaConceptoSeccionId;
                                            campo.PlantillaCartaCampoId = iteml.PlantillaCartaCampoId;
                                            campo.NombreCampo = iteml.NombreCampo;
                                            campo.Longitud = iteml.TipoCampo.Longitud;
                                            campo.Tipo = iteml.TipoCampo.Tipo;
                                            campo.ValorDefecto = item.ValorDefecto;
                                            campo.DatoValor = item.ValorDefecto;
                                        }
                                        if (iteml.NombreCampo === 'OcultaCDP') {
                                            campo.DatoValor = campo.DatoValor === 'true' ? true : false;
                                        }

                                        return campo;
                                    })
                                }

                                cartaSeccion.push(plantillaseccion);

                            });


                        }

                        vm.carta.ListaCartaSecciones = cartaSeccion;

                    }
                    else {
                        vm.carta.Id = 0;
                        vm.carta.TramiteId = vm.tramiteId;
                        vm.carta.Proceso = vm.numeroTramite;
                        vm.carta.Entidad = vm.entidad;
                        vm.carta.TipoId = vm.tipoTramite;
                        var cartaSeccion = [];

                        var plantillaseccion = {
                            Id: vm.plantillaCarta.PlantillaSecciones[0].Id,
                            ListaCartaCampos: vm.listaCampos.map(function (iteml) {
                                var campo = {};

                                campo.Id = 0;
                                //campo.Orden = campotmp.Orden;
                                campo.CartaConceptoSeccionId = iteml.CartaConceptoSeccionId;
                                campo.PlantillaCartaCampoId = iteml.Id;
                                campo.NombreCampo = iteml.NombreCampo;
                                campo.Longitud = iteml.TipoCampo.Longitud;
                                campo.Tipo = iteml.TipoCampo.Tipo;
                                campo.ValorDefecto = iteml.ValorDefecto;
                                campo.DatoValor = iteml.TextoDefecto;

                                if (iteml.NombreCampo === 'OcultaCDP') {
                                    campo.DatoValor = iteml.TextoDefecto === 'true' ? true : false;
                                }
                                return campo;
                            })

                        }
                        cartaSeccion.push(plantillaseccion);
                        vm.carta.ListaCartaSecciones = cartaSeccion;

                    }
                    var texto2 = document.getElementById('texto2');
                    texto2.innerHTML = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[7].DatoValor;
                    var texto3 = document.getElementById('texto3');
                    texto3.innerHTML = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[8].DatoValor;


                });


        }

        function guardarCuerpoConcepto() {
            var parametros = vm.carta;
            if (parametros.ListaCartaSecciones[0].ListaCartaCampos[7].DatoValor.length > 8000) {
                swal('', "El Texto 2 es muy extenso", 'error');
            }
            else {
                if (parametros.ListaCartaSecciones[0].ListaCartaCampos[8].DatoValor.length > 8000) {
                    swal('', "El Texto 3 es muy extenso", 'error');
                }
                else {
                    cartaServicio.actualizarCartaDatosIniciales(parametros).then(function (response) {
                        if (response.data && (response.statusText === "OK" || response.status === 200)) {

                            if (response.data.Exito) {

                                vm.plantillaCarta = {};
                                vm.carta = {};
                                vm.listaCampos = [];


                                obtenerPlantillaCarta();
                                parent.postMessage("cerrarModal", window.location.origin);
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                guardarCapituloModificado();
                                vm.ActivarEditar();
                            } else {
                                swal('', response.data.Mensaje, 'warning');
                            }

                        } else {
                            swal('', "Error al realizar la operación", 'error');
                        }
                    });
                }
            }
        }

        function consultarAutorizacion() {

            cartaServicio.obtenerCuerpoConceptoAutorizacion(vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {

                    var datosRadicado = false;
                    cartaServicio.ConsultarCartaConcepto(vm.tramiteId).then(function (response) {
                        if (response.statusText === "OK" || response.status === 200) {
                            {
                                vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY")
                                vm.numeroRadicado = response.data.RadicadoEntrada == '' ? '0' : response.data.RadicadoEntrada;
                                datosRadicado = true;
                            }
                        }
                    });

                    response.data.forEach(autorizacion => {
                        var nacion = 0;
                        var propios = 0;

                        //if (!datosRadicado) {
                        //    vm.fechaRadicado = moment(autorizacion.FechaRadicacion).format("DD/MM/YYYY")
                        //    vm.numeroRadicado = autorizacion.NumeroRadicacion == '' ? '0' : autorizacion.NumeroRadicacion;
                        //    datosRadicado = true;
                        //}

                        if (autorizacion.TipoProyecto == 'Credito') //Credito
                        {
                            vm.datosAutCredito.entidadId = autorizacion.EntidadId;
                            vm.datosAutCredito.entidad = autorizacion.Entidad;
                            vm.recursoss = autorizacion.Recurso.split('*');
                            vm.valoress = autorizacion.Valor.split('*');
                            var datosFuentes = [];
                            for (var i = 0; i < vm.recursoss.length; i++) {
                                var datosf = {
                                    recurso: vm.recursoss[i].split('_')[0],
                                    valor: vm.valoress[i]
                                }
                                if (vm.recursoss[i].split('_')[1] == 'N')
                                    vm.RecursoNacion = true;
                                if (vm.recursoss[i].split('_')[1] == 'P')
                                    vm.RecursoPropios = true;

                                datosFuentes.push(datosf);
                                var valorCred = parseFloat(datosf.valor);
                                vm.totalCred = vm.totalCred + valorCred;
                            }
                            //if (vm.proyectosCredito.length == 0) {
                            var datosContra = {
                                programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto.split('Programa')[0],
                                tipoProyecto: autorizacion.TipoProyecto,
                                recursos: datosFuentes,
                                // valor: autorizacion.Valor,
                                item: vm.proyectosCredito.length + 1
                            }
                            vm.proyectosCredito.push(datosContra);
                           
                        }
                    });

                    if (vm.proyectosContraCredito.length == 0) {
                        vm.totalContra = vm.totalCred;
                    }

                    if (vm.RecursoPropios && vm.RecursoNacion) {
                        vm.tipoRecurso = 'Nación y Propios'
                    }
                    if (vm.RecursoPropios && !vm.RecursoNacion) {
                        vm.tipoRecurso = 'Propios'
                    }
                    if (!vm.RecursoPropios && vm.RecursoNacion) {
                        vm.tipoRecurso = 'Nación'
                    }
                }

            }, error => {
                console.log(error);
            });
        }

        function consultarCDP() {
            cartaServicio.obtenerCuerpoConceptoCDP(vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.gridCDPGR.columnDefs = [];
                    response.data.forEach(archivo => {

                        vm.datosCDPGR.push({
                            cdp: archivo.CDP,
                            fechaCDP: moment(archivo.FechaCDP).format("DD/MM/YYYY"),
                            valorCDP: archivo.ValorCDP,
                        });
                    });

                    vm.gridCDPGR.showHeader = true;
                    vm.gridCDPGR.columnDefs = vm.columnDefCDPRP;
                    vm.gridCDPGR.data = vm.datosCDPGR;
                }
            }, error => {
                console.log(error);
            });
        }

        function obtenerFechaSinHoras(fecha) {
            let day = fecha.getDate()
            let month = fecha.getMonth() + 1
            let year = fecha.getFullYear()
            vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY")
        }


        vm.getTableHeightCDPGR = function () {
            var rowHeight = 30;
            var headerHeight = 50;
            return {
                height: (((vm.datosCDPGR.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };
        }

        vm.getProgramaHeight = function () {
            var longitud = vm.proyectosValoresContraCredito.length;

            var height = vm.heigprograma + (longitud * 41);
            return {
                height: height + "px"
            }
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
                //panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                //obtenerDatosIniciales();
                obtenerPlantillaCarta();
            }
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabled == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        function consultarConvenio() {
            vm.listaconvenios = [];
            datosAdicionDonacionServicio.ObtenerDatosAdicionDonacion(vm.tramiteId)
                .then(function (response) {
                    var datosAdicion = "";
                    if (response.data != null && response.data != "") {
                        var arreglolistaDatos = jQuery.parseJSON(response.data);
                        var arreglolistaDatosAdicion = jQuery.parseJSON(arreglolistaDatos);

                        if (arreglolistaDatosAdicion[0].ConvenioId > 0) {
                            for (var ls = 0; ls < arreglolistaDatosAdicion.length; ls++) {                               
                                vm.listaconvenios.push({
                                    numeroConv: arreglolistaDatosAdicion[ls].NumeroConvenio,
                                    objetoConv: arreglolistaDatosAdicion[ls].ObjetoConvenio,
                                    fechasConv: arreglolistaDatosAdicion[ls].periodo,
                                    totalConv: arreglolistaDatosAdicion[ls].ValorConvenio,
                                    montoVigConv: arreglolistaDatosAdicion[ls].ValorConvenioVigencia,
                                    aportanteConv: arreglolistaDatosAdicion[ls].NombreDonante
                                });
                            }
                            
                        }
                    }
                });


            //$sessionStorage.listaDatosIncorporacion.forEach(conv => {
            //    vm.listaconvenios.push({
            //        numeroConv: conv.NumeroConvenio,
            //        objetoConv: conv.ObjetoConvenio,
            //        fechasConv: conv.Periodo,
            //        totalConv: conv.ValorConvenio,
            //        montoVigConv: conv.ValorConvenioVigencia,
            //        aportanteConv: conv.NombreDonante
            //    });

            //});

        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }


        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }

    }

    angular.module('backbone').component('cuerpoTad', {
        templateUrl: "src/app/formulario/ventanas/tramiteAdicionDonacion/Componentes/conceptoTad/elaborarConcepto/Carta/cuerpoTad.html",
        controller: cuerpoTadController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            tipotramiteid: '@',
            tramiteid: '@',
            nivel: '@',
            rol: '@',
            section: '@',
            nombrecomponentepaso: '@',
            deshabilitar: '@',
            rolanalista: '@',
            novalidarconcepto: '@'
        }
    });


})();