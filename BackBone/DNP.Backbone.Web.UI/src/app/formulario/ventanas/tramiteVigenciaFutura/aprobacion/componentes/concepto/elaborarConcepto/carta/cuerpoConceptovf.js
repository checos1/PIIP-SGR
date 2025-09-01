    (function () {
    'use strict';

    cuerpoConceptovfController.$inject = [
        'sesionServicios',
        '$uibModal',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'cartaServicio',
        'servicioAcciones',
        'justificacionCambiosServicio',
    ];

    function cuerpoConceptovfController(
        sesionServicios,
        $uibModal,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        cartaServicio,
        servicioAcciones,
        justificacionCambiosServicio,
    ) {
        var vm = this;
        vm.init = init;
        vm.user = {};
        vm.lang = "es";
        vm.EntityTypeCatalogOptionId = 0;
        vm.conceptoValor = 'Concepto favorable';
        vm.tipoTramite = 4;
        vm.tipoTramite = $sessionStorage.tipoTramiteId == undefined ? vm.tipoTramite : $sessionStorage.tipoTramiteId;
        vm.nombreSccion = vm.tipoTramite == 4 ? 'Cuerpo VFO' : vm.tipoTramite == 5 ? 'Cuerpo VFO-AoPC' : 'Cuerpo' ;
        vm.tramiteId = $sessionStorage.tramiteId;
        vm.numeroTramite = $sessionStorage.numeroTramite;
        vm.entidad = $sessionStorage.nombreProyecto != undefined ? $sessionStorage.nombreProyecto : $sessionStorage.usuario.permisos.Entidades[0].NombreEntidad;
        vm.fechaFinalVFO = $sessionStorage.FechaFinal != undefined ? $sessionStorage.FechaFinal : "";

        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY");
        vm.numeroRadicado = 0;
        obtenerFechaSinHoras(new Date());
        vm.totalContra = 0;
        vm.totalCred = 0;
        vm.tipoRecurso = "Nación";
        vm.RecursoNacion = false;
        vm.RecursoPropios = false;

        vm.disabled = true;
        vm.Editar = 'EDITAR';
        vm.mostrarConstantes = false;
        vm.heigprograma = 150;
        vm.mostrarCDP = true;
        vm.mostrarCRP = false;

        /*variables objetos*/

        vm.plantillaCarta = {};
        vm.carta = {};
        vm.listaCampos = [];
        vm.listaCRP = [];

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

        vm.proyectosValoresContraCredito = [];

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

            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '' && vm.tramiteid !== undefined && vm.tramiteid !== null) {
                    vm.tramiteId = vm.tramiteid;
                    vm.tipoTramite = vm.tipotramiteid;
                    vm.abrirpanel();
                    consultarCDP();
                    consultarAutorizacion();
                    obtenerPlantillaCarta();
                }
            });
        }

        vm.abrirpanel = function () {
            var panel = document.getElementById('panelcuerpoconcepto');;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                if (vm.tipoTramite==4)
                    panel.style.maxHeight = "3500px";
                if (vm.tipoTramite == 5)
                    panel.style.maxHeight = "4000px";
                if (vm.tipoTramite == 18)
                    panel.style.maxHeight = "4000px";
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
           
                var idPlantillaSeccion = vm.plantillaCarta.PlantillaSecciones[0].Id;
                cartaServicio.obtenerDatosCartaPorSeccion(vm.tramiteId, idPlantillaSeccion)
                    .then(resultado => {
                        if (resultado != undefined && resultado.data != null && resultado.data.length > 0) {
                            var tmp = resultado.data[0];
                            vm.carta.Id = tmp.Id;
                            vm.carta.TramiteId = tmp.TramiteId;
                            vm.carta.Proceso = tmp.Proceso;
                            vm.carta.Entidad = tmp.Entidad;
                            vm.entidad = vm.carta.Entidad;
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
                        var texto4 = document.getElementById('texto4');
                        texto4.innerHTML = vm.carta.ListaCartaSecciones[0].ListaCartaCampos[12].DatoValor;
                    });


            cartaServicio.ConsultarCartaConcepto(vm.tramiteId).then(function (response) {
                if (response.statusText === "OK" || response.status === 200) {
                    {
                        vm.fechaRadicado = moment($sessionStorage.InstanciaSeleccionada.fechaPaso).format("DD/MM/YYYY")
                        vm.numeroRadicado = response.data.RadicadoEntrada == '' ? '0' : response.data.RadicadoEntrada;
                        
                    }
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
                    if (parametros.ListaCartaSecciones[0].ListaCartaCampos[12].DatoValor.length > 8000) {
                        swal('', "El Texto 4 es muy extenso", 'error');
                    }
                    else {
                        cartaServicio.actualizarCartaDatosIniciales(parametros).then(function (response) {
                            if (response.data && (response.statusText === "OK" || response.status === 200)) {

                                if (response.data.Exito) {

                                    vm.plantillaCarta = {};
                                    vm.carta = {};
                                    vm.listaCampos = [];
                                    guardarCapituloModificado();
                                    
                                    vm.ActivarEditar();
                                    parent.postMessage("cerrarModal", window.location.origin);
                                    utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito");
                                   

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
        }

        function consultarAutorizacion() {
            //var datosRadicado = false;
            //cartaServicio.ConsultarCartaConcepto(vm.tramiteId).then(function (response) {
            //    if (response.statusText === "OK") {
            //        {
            //            vm.fechaRadicado = moment(response.data.FechaCreacion).format("DD/MM/YYYY")
            //            vm.numeroRadicado = response.data.RadicadoEntrada == '' ? '0' : response.data.RadicadoEntrada;
            //            datosRadicado = true;
            //        }
            //    }
            //});
            cartaServicio.obtenerCuerpoConceptoAutorizacion(vm.tramiteId).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    var datosRadicado = false;
                    response.data.forEach(autorizacion => {
                        var nacion = 0;
                        var propios = 0;
                        var constnacion = 0;
                        var constpropios = 0;
                        var vigencia = '';
                        var recurso = '';
                        var deflactor = 0;

                        //VFO
                      //  if (autorizacion.TipoProyecto.split('-')[0] == 'No Aplica') {

                            vigencia = autorizacion.Recurso.split('-').length > 1 ? autorizacion.Recurso.split('-')[1] : '';
                            recurso = autorizacion.Recurso.split('-')[0];
                            deflactor = autorizacion.TipoProyecto.split('-').length > 1 ? autorizacion.TipoProyecto.split('-')[1] : 0;

                            if (vm.proyectosContraCredito.length == 0) {
                                var datosContra = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto.split('-')[0],
                                    valorNacionContra: 0, valorPropiosContra: 0,
                                    valorConstantaNacion: 0, valorConstantePropios: 0,
                                    item: 1,
                                    vigencia: vigencia,
                                    recurso: recurso
                                }
                                vm.proyectosContraCredito.push(datosContra);
                            }
                            if (vm.proyectosValoresContraCredito.length == 0) {
                                var datosValores = {
                                    valorNacionContra: 0, valorPropiosContra: 0,
                                    valorConstantaNacion: 0, valorConstantePropios: 0,
                                    item: 1,
                                    vigencia: vigencia,
                                    recurso: recurso
                                }
                                vm.proyectosValoresContraCredito.push(datosValores);
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
                            //constantes
                            if (recurso == 'Aprobado Constante Nacion') {                                
                                vm.datosAutContraCredito.valorConstantaNacion = parseFloat(autorizacion.Valor);
                                constnacion = parseFloat(autorizacion.Valor);
                                nacion = constnacion * deflactor;
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorConstantaNacion;
                                vm.RecursoNacion = true;
                                vm.mostrarConstantes = true;
                            }
                            if (recurso == 'Aprobado Constante Propios') {                                
                                vm.datosAutContraCredito.valorConstantePropios = parseFloat(autorizacion.Valor);
                                constpropios = parseFloat(autorizacion.Valor);
                                propios = constpropios * deflactor;
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorConstantePropios;
                                vm.RecursoPropios = true;
                                vm.mostrarConstantes = true;
                            }
                            if (recurso == 'NACION') {
                                vm.datosAutContraCredito.valorNacionContra = parseFloat(autorizacion.Valor);                                
                                nacion = parseFloat(autorizacion.Valor);
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorNacionContra;
                                vm.RecursoNacion = true;
                            }
                            if (recurso == 'PROPIOS') {
                                vm.datosAutContraCredito.valorPropiosContra = parseFloat(autorizacion.Valor);
                                propios = parseFloat(autorizacion.Valor);
                                vm.totalContra = vm.totalContra + vm.datosAutContraCredito.valorPropiosContra;
                                vm.RecursoPropios = true;
                            }
                            

                            var loEncontro = false;
                            var contador = 0;
                            var posicion = 0;
                            var loEncontroV = false;
                            var contadorV = 0;
                            var posicionV = 0;
                            vm.proyectosContraCredito.forEach(x => {
                                if (x.programaCodigo == autorizacion.ProgramaCodigo && x.subProgramaCodigo == autorizacion.SubProgramaCodigo
                                   // && x.tipoProyecto === autorizacion.TipoProyecto.split('-')[0]
                                    && x.codigoPresupuestal === autorizacion.CodigoPresupuestal) {
                                    loEncontro = true;
                                    posicion = contador;                                    
                                }
                                else {
                                    loEncontro = false;
                                }
                                contador++;
                            });
                            //arma el arreglo de programa, subprograma
                            if (loEncontro) {
                                vm.proyectosContraCredito[posicion].valorNacionContra = vm.proyectosContraCredito[posicion].valorNacionContra + nacion;
                                vm.proyectosContraCredito[posicion].valorPropiosContra = vm.proyectosContraCredito[posicion].valorPropiosContra + propios;
                                vm.proyectosContraCredito[posicion].valorConstantaNacion = vm.proyectosContraCredito[posicion].valorConstantaNacion + constnacion;
                                vm.proyectosContraCredito[posicion].valorConstantePropios = vm.proyectosContraCredito[posicion].valorConstantePropios + constpropios;
                                vm.proyectosContraCredito[posicion].vigencia = vigencia;
                            }
                            else {
                                var datosContra = {
                                    programaCodigo: autorizacion.ProgramaCodigo, programa: autorizacion.Programa,
                                    subProgramaCodigo: autorizacion.SubProgramaCodigo, subPrograma: autorizacion.SubPrograma,
                                    codigoPresupuestal: autorizacion.CodigoPresupuestal, nombreProyecto: autorizacion.NombreProyecto,
                                    tipoProyecto: autorizacion.TipoProyecto.split('-')[0],
                                    valorNacionContra: nacion, valorPropiosContra: propios,
                                    valorConstantaNacion: constnacion, valorConstantePropios: constpropios,
                                    item: vm.proyectosContraCredito.length + 1,
                                    vigencia: vigencia,
                                    recurso: recurso
                                }
                                vm.proyectosContraCredito.push(datosContra);
                            }


                            vm.proyectosValoresContraCredito.forEach(y => {
                                if (y.vigencia == vigencia) {
                                    loEncontroV = true;
                                    posicionV = contadorV;
                                }
                                else {
                                    loEncontroV = false;
                                }
                                contadorV++;
                            });
                            //armas el arreglo de valores
                            if (loEncontroV) {
                                vm.proyectosValoresContraCredito[posicionV].valorNacionContra = vm.proyectosValoresContraCredito[posicionV].valorNacionContra + nacion;
                                vm.proyectosValoresContraCredito[posicionV].valorPropiosContra = vm.proyectosValoresContraCredito[posicionV].valorPropiosContra + propios;
                                vm.proyectosValoresContraCredito[posicionV].valorConstantaNacion = vm.proyectosValoresContraCredito[posicionV].valorConstantaNacion + constnacion;
                                vm.proyectosValoresContraCredito[posicionV].valorConstantePropios = vm.proyectosValoresContraCredito[posicionV].valorConstantePropios + constpropios;
                                vm.proyectosValoresContraCredito[posicionV].vigencia = vigencia;
                                vm.proyectosValoresContraCredito[posicionV].recurso = recurso;
                            }
                            else {
                                var datosValores = {
                                    valorNacionContra: nacion, valorPropiosContra: propios,
                                    valorConstantaNacion: constnacion, valorConstantePropios: constpropios,
                                    item: vm.proyectosValoresContraCredito.length + 1,
                                    vigencia: vigencia,
                                    recurso: recurso
                                }
                                vm.proyectosValoresContraCredito.push(datosValores);


                                //vm.getProgramaHeight();
                                
                            }

                       // }//Credito
                        
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
            if ($sessionStorage.listaCDP != undefined && $sessionStorage.listaCDP.length > 0 && $sessionStorage.tipoTramiteId != 18) {
                vm.mostrarCDP = true;
                var listaCdp = $sessionStorage.listaCDP;
                //$sessionStorage.listaCRP
                listaCdp.forEach(archivo => {
                    vm.datosCDPGR.push({
                        cdp: archivo.NumeroCDP,
                        fechaCDP: moment(archivo.FechaCDP).format("DD/MM/YYYY"),
                        valorCDP: archivo.ValorCDP.split(',').length > 0 ? archivo.ValorCDP.split(',')[0] : archivo.ValorCDP
                    });
                });

                vm.gridCDPGR.showHeader = true;
                vm.gridCDPGR.columnDefs = vm.columnDefCDPRP;
                vm.gridCDPGR.data = _.sortBy(vm.datosCDPGR,'cdp');
            }
            else {
                //Ocultar CDP para AoPC
                vm.mostrarCDP = false;

                //cartaServicio.obtenerCuerpoConceptoCDP(vm.tramiteId).then(function (response) {
                //    if (response === undefined || typeof response === 'string') {
                //        vm.mensajeError = response;
                //        utilidades.mensajeError(response);
                //    } else {
                //        vm.gridCDPGR.columnDefs = [];
                //        response.data.forEach(archivo => {

                //            vm.datosCDPGR.push({
                //                cdp: archivo.CDP,
                //                fechaCDP: moment(archivo.FechaCDP).format("DD/MM/YYYY"),
                //                valorCDP: archivo.ValorCDP,
                //            });
                //        });

                //        vm.gridCDPGR.showHeader = true;
                //        vm.gridCDPGR.columnDefs = vm.columnDefCDPRP;
                //        vm.gridCDPGR.data = _.sortBy(vm.datosCDPGR, 'cdp');
                //    }
                //}, error => {
                //    console.log(error);
                //});
            }

            if ($sessionStorage.listaCRP != undefined && $sessionStorage.tipoTramiteId != 18 ) {
                vm.mostrarCRP = true;
                vm.listaCRP = $sessionStorage.listaCRP;
            }
            else {
                vm.mostrarCRP = false;
            }
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
            const span = document.getElementById('id-capitulo-solicitarconceptocuerpo');
            vm.seccionCapitulo = span.textContent;


        }

    }

    angular.module('backbone').component('cuerpoConceptovf', {
        templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/aprobacion/componentes/concepto/elaborarConcepto/carta/cuerpoConceptovf.html",
        controller: cuerpoConceptovfController,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            guardadoevent: '&',
            rolanalista: '@',
        }
    });


})();