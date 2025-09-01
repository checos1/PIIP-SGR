(function () {
    'use strict';

    cuerpoConceptoController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'cartaServicio',
        'servicioAcciones'
    ];

    function cuerpoConceptoController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        cartaServicio,
        servicioAcciones
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.nombreSccion = 'Cuerpo';
        vm.conceptoValor = 'Concepto favorable';
        vm.tipoTramite = 1;
        vm.tipoTramite = $sessionStorage.TipoTramiteId;
        vm.tramiteId = $sessionStorage.TramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.usuario.permisos.Entidades[0].NombreEntidad;

        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY");
        vm.numeroRadicado=0;
        obtenerFechaSinHoras(new Date());
        vm.totalContra = 0;
        vm.totalCred = 0;
        vm.tipoRecurso = "";
        vm.RecursoNacion = false;
        vm.RecursoPropios = false;

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


        function init() {            
            consultarCDP();
            consultarAutorizacion();
            obtenerPlantillaCarta();
        }

        vm.abrirpanel = function () {
            if (vm.plantillaCarta === null) {
                swal('', "No se ha creado la carta", 'error');
            }
            else {
                var acc = document.getElementById('divcuerpoconcepto');
                var i;
                var rotated = false;


                acc.classList.toggle("active");
                var panel = acc.nextElementSibling;
                if (panel.style.maxHeight) {
                    panel.style.maxHeight = null;
                } else {
                    panel.style.maxHeight = panel.scrollHeight + "px";
                }
                var div = document.getElementById('u4_imgcuerpoconcepto'),
                    deg = vm.rotated ? 180 : 0;
                div.style.webkitTransform = 'rotate(' + deg + 'deg)';
                div.style.mozTransform = 'rotate(' + deg + 'deg)';
                div.style.msTransform = 'rotate(' + deg + 'deg)';
                div.style.oTransform = 'rotate(' + deg + 'deg)';
                div.style.transform = 'rotate(' + deg + 'deg)';
                vm.rotated = !vm.rotated;
            }
        }

        function obtenerPlantillaCarta() {
            cartaServicio.obtenerPlantillaCarta(vm.nombreSccion, vm.tipoTramite)
                .then(resultado => {
                    if (resultado != undefined && resultado.data) {
                        var PlantillaSecciones = [];
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
            cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, 2)
                .then(resultado => {
                    if (resultado != undefined && resultado.data != null  && resultado.data.length > 0) {
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
                                    campo.DatoValor = iteml.TextoDefecto === 'true'?true:false;
                                }
                                return campo;
                            })

                        }
                        cartaSeccion.push(plantillaseccion);
                        vm.carta.ListaCartaSecciones = cartaSeccion;
                        
                    }

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
            cartaServicio.obtenerCuerpoConceptoAutorizacion(vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {

                    var datosRadicado = false;
                    response.data.forEach(autorizacion => {
                        var nacion = 0;
                        var propios = 0;

                        //if (!datosRadicado) {
                        //    vm.fechaRadicado = moment(autorizacion.FechaRadicacion).format("DD/MM/YYYY")
                        //    vm.numeroRadicado = autorizacion.NumeroRadicacion == '' ? '0' : autorizacion.NumeroRadicacion;
                        //    datosRadicado = true;
                        //}

                        //contracredito
                        if (autorizacion.TipoProyecto == 'Contracredito') {

                            if (vm.proyectosContraCredito.length == 0) {
                                var datosContra = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto,
                                    valorNacionContra: 0, valorPropiosContra: 0,
                                    item: 1
                                }
                                vm.proyectosContraCredito.push(datosContra);
                            }


                            vm.datosAutContraCredito.entidadId = autorizacion.EntidadId;
                            vm.datosAutContraCredito.entidad = autorizacion.Entidad;
                            vm.datosAutContraCredito.nombreProyecto = autorizacion.NombreProyecto;
                            vm.datosAutContraCredito.programaCodigo = autorizacion.ProgramaCodigo;
                            vm.datosAutContraCredito.programa = autorizacion.Programa;
                            vm.datosAutContraCredito.subProgramaCodigo = autorizacion.SubProgramaCodigo;
                            vm.datosAutContraCredito.subPrograma = autorizacion.SubPrograma;
                            vm.datosAutContraCredito.ProyectoId = autorizacion.ProyectoId;
                            vm.datosAutContraCredito.CodigoPresupuestal = autorizacion.CodigoPresupuestal;
                            if (autorizacion.Recurso == 'NACION') {
                                vm.datosAutContraCredito.valorNacionContra = parseFloat(autorizacion.Valor);
                                nacion = parseFloat(autorizacion.Valor);
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorNacionContra;
                                vm.RecursoNacion = true;
                            }
                            if (autorizacion.Recurso == 'PROPIOS') {
                                vm.datosAutContraCredito.valorPropiosContra = parseFloat(autorizacion.Valor);
                                propios = parseFloat(autorizacion.Valor);
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorPropiosContra;
                                vm.RecursoPropios = true;
                            }

                            var loEncontro = false;
                            var contador = 0;
                            var posicion = 0;
                            vm.proyectosContraCredito.forEach(x => {
                                if (x.programaCodigo == autorizacion.ProgramaCodigo && x.subProgramaCodigo == autorizacion.SubProgramaCodigo && x.tipoProyecto === autorizacion.TipoProyecto && x.codigoPresupuestal === autorizacion.CodigoPresupuestal) {
                                    loEncontro = true;
                                    posicion = contador;
                                }
                                else {
                                    loEncontro = false;
                                }
                                contador++;
                            });
                            if (loEncontro) {
                                vm.proyectosContraCredito[posicion].valorNacionContra = vm.proyectosContraCredito[posicion].valorNacionContra + nacion;
                                vm.proyectosContraCredito[posicion].valorPropiosContra = vm.proyectosContraCredito[posicion].valorPropiosContra + propios;
                            }
                            else {
                                var datosContra = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto,
                                    valorNacionContra: nacion, valorPropiosContra: propios,
                                    item: vm.proyectosContraCredito.length+1
                                }
                                vm.proyectosContraCredito.push(datosContra);
                            }
                        

                        }//Credito
                        else {

                            if (vm.proyectosCredito.length == 0) {
                                var datosContra = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto,
                                    valorNacionCred: 0, valorPropiosCred: 0,
                                    item: 1
                                }
                                vm.proyectosCredito.push(datosContra);
                            }

                            vm.datosAutCredito.entidadId = autorizacion.EntidadId;
                            vm.datosAutCredito.entidad = autorizacion.Entidad;
                            vm.datosAutCredito.nombreProyecto = autorizacion.NombreProyecto;
                            vm.datosAutCredito.programaCodigo = autorizacion.ProgramaCodigo;
                            vm.datosAutCredito.programa = autorizacion.Programa;
                            vm.datosAutCredito.subProgramaCodigo = autorizacion.SubProgramaCodigo;
                            vm.datosAutCredito.subPrograma = autorizacion.SubPrograma;
                            vm.datosAutCredito.ProyectoId = autorizacion.ProyectoId;
                            vm.datosAutCredito.CodigoPresupuestal = autorizacion.CodigoPresupuestal;
                            if (autorizacion.Recurso == 'NACION') {
                                vm.datosAutCredito.valorNacionCred = parseFloat(autorizacion.Valor);
                                nacion = parseFloat(autorizacion.Valor);
                                vm.totalCred = vm.totalCred + vm.datosAutCredito.valorNacionCred;
                                vm.RecursoNacion = true;
                            }
                            if (autorizacion.Recurso == 'PROPIOS') {
                                vm.datosAutCredito.valorPropiosCred = parseFloat(autorizacion.Valor);
                                propios = parseFloat(autorizacion.Valor);
                                vm.totalCred = vm.totalCred + vm.datosAutCredito.valorPropiosCred;
                                vm.RecursoPropios = true;
                            }


                            var loEncontro = false;
                            var contador = 0;
                            var posicion = 0;
                            vm.proyectosCredito.forEach(x => {
                                if (x.programaCodigo == autorizacion.ProgramaCodigo && x.subProgramaCodigo == autorizacion.SubProgramaCodigo && x.tipoProyecto === autorizacion.TipoProyecto && x.codigoPresupuestal === autorizacion.CodigoPresupuestal) {
                                    loEncontro = true;
                                    posicion = contador;
                                }
                                else {
                                    loEncontro = false;
                                }
                                contador++;
                            });
                            if (loEncontro) {
                                vm.proyectosCredito[posicion].valorNacionCred = vm.proyectosCredito[posicion].valorNacionCred + nacion;
                                vm.proyectosCredito[posicion].valorPropiosCred = vm.proyectosCredito[posicion].valorPropiosCred + propios;
                            }
                            else {
                                var datosCredito = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto,
                                    valorNacionCred: nacion, valorPropiosCred: propios,
                                    item: vm.proyectosCredito.length + 1
                                }
                                vm.proyectosCredito.push(datosCredito);
                            }

                        }
                    });


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
            //if ($sessionStorage.tabcdpactivo && !vm.renderizado) {
            //    window.setTimeout(function () {
            //        $(window).resize();
            //        $(window).resize();
            //        vm.renderizado = true;
            //    }, 100);
            //}
            return {
                height: (((vm.datosCDPGR.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };


        }

    }

    angular.module('backbone').component('cuerpoConcepto', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/carta/cuerpoConcepto.html",
        controller: cuerpoConceptoController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });


})();