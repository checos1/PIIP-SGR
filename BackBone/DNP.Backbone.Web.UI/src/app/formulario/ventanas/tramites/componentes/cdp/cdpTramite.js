(function () {
    'use strict';

    cdpTramiteController.$inject = [
        'backboneServicios',
        '$scope',
        'utilidades',
        '$sessionStorage',
        'constantesBackbone',
        'trasladosServicio',
        '$timeout',
        '$window',
        '$q'
    ];

    function cdpTramiteController(
        backboneServicios,
        $scope,
        utilidades,
        $sessionStorage,
        constantesBackbone,
        trasladosServicio,
        $timeout,
        $window,
        $q
    ) {

        /**variables globales */
        var vm = this;
        vm.user = {};
        vm.noJefePlaneacion = !$sessionStorage.jefePlaneacion;
        vm.disabledJefePlaneacion = $sessionStorage.jefePlaneacion;
        vm.idnivel = $sessionStorage.idNivel;

        vm.seleccionProyectos = false;
        vm.aprobacionEntidad = false;
        vm.elaboracionConcepto = false;
        vm.revisionConcepto = false;
        vm.aprobaciónTramite = false;

        vm.ProyectoId = $sessionStorage.ProyectoId;
        vm.TramiteId = $sessionStorage.TramiteId;
        vm.TipoRolId = $sessionStorage.TipoRolId;
        vm.NombreRol = $sessionStorage.TipoRolId ? $sessionStorage.TipoRolId : undefined;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.idFlujo = $sessionStorage.idFlujoIframe;
        vm.roles = {};
        vm.idRol = '';
        vm.idAccion = $sessionStorage.idAccion;
        vm.idProyectoRequisito = 0;

        vm.MontoTramiteNacion = $sessionStorage.MontoTramiteNacion ? $sessionStorage.MontoTramiteNacion : 0;
        vm.MontoTramitePropios = $sessionStorage.MontoTramitePropios ? $sessionStorage.MontoTramitePropios : 0;
        vm.ValorMontoProyecto = parseInt(limpiaNumero(vm.MontoTramiteNacion)) + parseInt(limpiaNumero(vm.MontoTramitePropios));

        vm.bool_aprobacion = $sessionStorage.idAccion === constantesBackbone.idAccionSeleccionProyectosTamites ? false : true;
        vm.valorFuente = formatearNumero(vm.ValorMontoProyecto);
        vm.exportExcel = exportExcel;


        /**variables locales */
        var cdpRP = {};
        vm.listaTipoRequisitos = [];
        vm.renderizado = false;
        vm.parametrosObjetosNegocioDto = {};
        vm.idsroles = [];



        /*variables configuación grids */
        vm.gridCDPGR = {};
        vm.scrollbars = {
            NEVER: 0,
            ALWAYS: 1,
            WHEN_NEEDED: 2
        }
        vm.columnDefCDPRP = [

            {
                field: 'tipo',
                visible: false
            },
            {
                field: 'numeroCDP',
                displayName: 'Número CDP ',
                cellClass: 'col-numcorto',
                enableHiding: false,
                minWidth: 200,
                cellTemplate: '<div><input style="text-align:center;" ng-value="row.entity.numeroCDP" placeholder="Digite Número CDP" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event, 1)"/></div>'
            },
            {
                field: 'valorCDP',
                displayName: 'Valor CDP Trámite',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div><input style="text-align:right;" ng-value="row.entity.valorCDP" placeholder="Digite el valor" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event, 2)"  /></div>',
                cellClass: 'col-valor'

            },
            {
                field: 'fechaCDP',
                displayName: 'Fecha CDP',
                cellClass: 'col-numcorto',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div><input type="date"  style="text-align:center;"  ng-value="row.entity.fechaCDP" placeholder="yyyy-MM-dd" min="2013-01-01" ng-blur="grid.appScope.actualizaFila($event, 3)"/></div>'
            },
            {
                field: 'descripcion',
                displayName: 'Descripción CDP',
                cellClass: 'col-texto',
                enableHiding: false,
                minWidth: 200,
                cellTemplate: '<div><input style="text-align:justify;" ng-value="row.entity.descripcion" placeholder="Digite descipción" ng-blur="grid.appScope.actualizaFila($event, 4)"/></div>'
            },
            {
                field: 'valortotalCDP',
                displayName: 'Valor Total CDP Trámite',
                enableHiding: false,
                minWidth: 150,
                cellTemplate: '<div><input style="text-align:right;" ng-value="row.entity.valortotalCDP" placeholder="Digite Descripción CDP" ng-keypress="grid.appScope.onKeyPress($event)" ng-blur="grid.appScope.actualizaFila($event,  5)"  /></div>',
                cellClass: 'col-valor'


            },
            {
                field: 'unidadejecutora',
                displayName: 'Unidad Ejecutora',
                cellClass: 'col-texto',
                enableColumnMenu: false,
                minWidth: 200,
                cellTemplate: '<div><input style="text-align:justify;" ng-value="row.entity.unidadejecutora" placeholder="Digite Unidad Ejecutora" ng-blur="grid.appScope.actualizaFila($event, 6)"/></div>'
            },
            {
                field: 'eliminar',
                displayName: 'Eliminar',
                cellClass: 'col-numcorto',
                enableColumnMenu: false,
                cellTemplate: '<div><button has-claim   class="btnaccion" ng-click="grid.appScope.eliminarRegistro_onClick($event, row.entity)" tooltip-placement="auto" uib-tooltip="Eliminar registro" ng-show="grid.appScope.seleccionProyectos || grid.appScope.aprobacionEntidad"><span><img class= "grid-icon-accion" src="Img/Eliminar.svg" /></span ></button ></div>',

            },
            {
                field: "idproyectotramite",
                visible: false

            },
            {
                field: "idproyectoRequisitotramite",
                visible: false

            },
            {
                field: "idtiporequito",
                visible: false

            },
            {
                field: "idvalorCDP",
                visible: false

            },
            {
                field: "idvaloraportaCDP",
                visible: false

            }
        ];
        vm.datosCDPGR = [];

        /*variables archivo */

        vm.datos = [];
        $scope.files = [];
        $scope.nombreArchivo = '';



        /*variables metodos */
        vm.cargarProyectoRequisitosPorTramite = cargarProyectoRequisitosPorTramite;
        vm.addFilaCDP = addFilaCDP;
        vm.archivoSeleccionado = archivoSeleccionado;
        vm.limpiarArchivo = limpiarArchivo;
        vm.actualizarCDP = actualizarCDP;
        vm.ObtenerTiposRequisito = ObtenerTiposRequisito;


        /*Carga grids */

        vm.gridCDPGR = {
            enableSorting: true,
            enableRowSelection: true,
            enableFullRowSelection: true,
            multiSelect: true,
            enableRowHeaderSelection: false,
            enableColumnMenus: false,
            columnDefs: vm.columnDefCDPRP,
            data: vm.datosCDPGR
        };

        vm.gridCDPGR.appScopeProvider = vm;
        vm.gridCDPGR.onRegisterApi = function (gridApi) {
            vm.gridApi = gridApi;
        };

        /**Ejecucion de metodos */

        vm.inicializar = function () {
            inicializarCarga();
            vm.parametrosObjetosNegocioDto.EntidadId = $sessionStorage.EntidadId;
            vm.parametrosObjetosNegocioDto.IdUsuarioDNP = $sessionStorage.usuario.permisos.IdUsuarioDNP;
            vm.parametrosObjetosNegocioDto.UsuarioDNP = '';
            $sessionStorage.usuario.roles.map(function (item) {
                vm.idsroles.push(item.IdRol);
            });
            vm.parametrosObjetosNegocioDto.IdsRoles = vm.idsroles;
            trasladosServicio.obtenerInstanciasPermiso(vm.parametrosObjetosNegocioDto).then(function (result) {
                if (result !== null && result.data.length > 0)
                    var listaAutorizaciones = result.data;
                result.data.map(function (item) {
                    if (item.InstanciaId === vm.IdInstancia && item.FlujoId === vm.idFlujo) {
                        vm.idsroles.map(function (itemRol) {
                            if (itemRol === item.RolId)
                                vm.idRol = item.RolId;
                        });
                    }
                });

            });

            if (vm.idnivel == constantesBackbone.idNivelSeleccionProyectos.toLowerCase() && !vm.disabledJefePlaneacion)
                vm.seleccionProyectos = true;
            else if (vm.idnivel == constantesBackbone.idNivelAprobacionEntidad.toLowerCase() && !vm.disabledJefePlaneacion)
                vm.aprobacionEntidad = true;
            else if (vm.idnivel == constantesBackbone.idNivelElaboracionConcepto.toLowerCase())
                vm.elaboracionConcepto = true;
            else if (vm.idnivel == constantesBackbone.idNivelRevisionConcepto.toLowerCase())
                vm.revisionConcepto = true;
            else if (vm.idnivel == constantesBackbone.idNivelAprobacionTramite.toLowerCase())
                vm.aprobaciónTramite = false;

        }

        function inicializarCarga() {
            cargalistatiposrequisitos().then(function (status) {
                cargalistarequisitostramite(true).then(function () {

                });

            }, function (error) {


            });

        }

        function cargalistatiposrequisitos() {
            var deferred = $q.defer();
            try {
                ObtenerTiposRequisito();
                window.setTimeout(function () {
                    $(window).resize();
                    $(window).resize();
                }, 100);
                deferred.resolve("done");
            } catch (err) {

                deferred.reject(err);
            }
            return deferred.promise;
        }

        function cargalistarequisitostramite(valor) {
            var deferred = $q.defer();
            try {
                cargarProyectoRequisitosPorTramite(valor);
                deferred.resolve("done");
            } catch (err) {

                deferred.reject(err);
            }
            return deferred.promise;
        }



        vm.getTableHeightCDPGR = function () {
            var rowHeight = 30;
            var headerHeight = 50;
            if ($sessionStorage.tabcdpactivo && !vm.renderizado) {
                window.setTimeout(function () {
                    $(window).resize();
                    $(window).resize();
                    vm.renderizado = true;
                }, 100);
            }
            return {
                height: (((vm.datosCDPGR.length + 1) * rowHeight + headerHeight) + 30) + "px"
            };


        }

        vm.onKeyPress = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();;
            }

        }

        vm.eliminarRegistro_onClick = function (event, fila) {

            var indice = vm.gridCDPGR.data.findIndex(x => x.numeroCDP === fila.numeroCDP);

            vm.gridCDPGR.data.splice(indice, 1);


        }

        vm.actualizaFila = function (event, id) {
            var valor = '';
            
            
            const btnScope = angular.element(event.target).scope();
            const entity = (btnScope !== undefined && btnScope !== null) ? /*Acceder al scope de la fila y obtener el elemento enlazado*/btnScope.$parent.$parent.row.entity : null;
            if (id === 1) {
                $(event.target).val(function (index, value) {
                    entity.numeroCDP = formatearNumero(value);
                });
            }
            else if (id === 2) {
                $(event.target).val(function (index, value) {
                    entity.valorCDP = formatearNumero(value);
                });
            }
            else if (id === 3) {
                $(event.target).val(function (index, value) {
                    entity.fechaCDP = new Date(value + "T00:00:00").toLocaleDateString('en-CA');
                });
            }
            else if (id === 4) {
                $(event.target).val(function (index, value) {
                    entity.descripcion = (value);
                });
            }
            else if (id === 5) {
                $(event.target).val(function (index, value) {
                    entity.valortotalCDP = formatearNumero(value);
                });
            }
            else if (id === 6) {
                $(event.target).val(function (index, value) {
                    entity.unidadejecutora = (value);
                });
            }


        }

        function actualizarCDP() {
            var tramiteRequisitosDto = [];
            $sessionStorage.usuario.roles.map(function (item) {
                vm.roles += (item.Id + ',');
            });


            if (validaTabla()) {
                vm.gridCDPGR.data.map(function (item, index) {

                    var tramiterequisito = {
                        Descripcion: item.descripcion
                        , FechaCDP: item.fechaCDP
                        , IdPresupuestoValoresCDP: item.idpresupuestovaloresCDP
                        , IdPresupuestoValoresAportaCDP: item.idpresupuestovaloresaportaCDP
                        , IdProyectoRequisitoTramite: item.idproyectoRequisitotramite
                        , IdProyectoTramite: item.idproyectotramite
                        , IdTipoRequisito: 1 //item.idtiporequito
                        , NumeroCDP: limpiaNumero(item.numeroCDP)
                        , Tipo: 'CDP'
                        , UnidadEjecutora: item.unidadejecutora
                        , ValorCDP: limpiaNumero(item.valorCDP)
                        , ValorTotalCDP: limpiaNumero(item.valortotalCDP)
                        , IdValorTotalCDP: item.idvalorCDP
                        , IdValorAportaCDP: item.idvaloraportaCDP
                        , IdProyecto: vm.ProyectoId
                        , IdTramite: vm.TramiteId
                        , IdTipoRol: vm.TipoRolId
                        , IdRol: vm.idRol
                    }
                    tramiteRequisitosDto.push(tramiterequisito);
                });

                var x = trasladosServicio.actualizarTramitesRequisitos(tramiteRequisitosDto).then(function (response) {
                    if (response.data && (response.statusText === "OK" || response.status === 200)) {
                        parent.postMessage("cerrarModal", window.location.origin);
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
            }

        }

        function ObtenerTiposRequisito() {

            var tr = trasladosServicio.ObtenerTiposRequisito().then(function (result) {
                vm.listaTipoRequisitos = result.data;


            });
        }

        function cargarProyectoRequisitosPorTramite() {
            limpiaCdpRP();
            var proyectotmp = vm.ProyectoId === undefined ? 0 : vm.ProyectoId;
            if (vm.TramiteId != undefined) {
                var tr = trasladosServicio.obtenerProyectoRequisitosPorTramite(proyectotmp, vm.TramiteId).then(function (result) {
                    if (result.data.length > 0)
                        AgregarRequisitosPorTramite(result);
                });
            }
        }

        function AgregarRequisitosPorTramite(result) {
            cdpRP.idproyectoRequisitotramite = 0;
            if (result.data !== undefined && result.data.length > 0) {
                result.data.map(function (item, index) {
                    if (cdpRP.idproyectoRequisitotramite > 0)
                        vm.datosCDPGR.push(cdpRP);
                    limpiaCdpRP();
                    cdpRP.idproyectotramite = item.TramiteProyectoId;
                    cdpRP.idproyectoRequisitotramite = item.Id;
                    cdpRP.descripcion = item.Descripcion;
                    cdpRP.numeroCDP = item.Numero;
                    cdpRP.fechaCDP = new Date(item.Fecha).toLocaleDateString('en-CA'); //  formatoFecha(item.Fecha); 
                    cdpRP.unidadejecutora = item.UnidadEjecutora
                    if (item.ListaTiposRequisito !== undefined && item.ListaTiposRequisito.length > 0) {
                        item.ListaTiposRequisito.map(function (itemfuente, indexfuente) {
                            cdpRP.idtiporequito = 1; //itemfuente.Id;
                            cdpRP.tipo = "CDP"; // itemfuente.TipoRequisito;


                            if (itemfuente.ListaValores !== undefined && itemfuente.ListaValores.length > 0) {
                                itemfuente.ListaValores.map(function (itemvalor, indexvalor) {
                                    if (itemvalor.TipoValor.TipoValorFuente === 'Valor Aporta') {
                                        cdpRP.valorCDP = formatearNumero(itemvalor.Valor);
                                        cdpRP.idvaloraportaCDP = itemvalor.TipoValor.Id;
                                    }
                                    else if (itemvalor.TipoValor.TipoValorFuente === 'Valor') {
                                        cdpRP.valortotalCDP = formatearNumero(itemvalor.Valor);
                                        cdpRP.idvalorCDP = itemvalor.TipoValor.Id;
                                    }

                                });
                            }

                        });

                    }
                });
            }
            vm.datosCDPGR.push(cdpRP);
        }

        function limpiaCdpRP() {
            cdpRP = {
                idproyectotramite: 0,
                idproyectoRequisitotramite: 0,
                idtiporequito: 0,
                idpresupuestovaloresCDP: 0,
                idpresupuestovaloresaportaCDP: 0,
                tipo: '',
                numeroCDP: '',
                valorCDP: 0,
                fechaCDP: '',
                descripcion: '',
                valortotalCDP: 0,
                unidadejecutora: ''
            }
        }

        function addFilaCDP() {
            $timeout(function () {
                $scope.$apply(function () {
                    limpiaCdpRP();
                    cdpRP.idproyectotramite = 0;
                    cdpRP.idproyectoRequisitotramite = 0;
                    cdpRP.descripcion = '';
                    cdpRP.numeroCDP = '';
                    cdpRP.fechaCDP = '';
                    cdpRP.idtiporequito = '';
                    cdpRP.tipo = '';
                    cdpRP.valorCDP = '0';
                    cdpRP.idvaloraportaCDP = 0;
                    cdpRP.valortotalCDP = '0';
                    cdpRP.idvalorCDP = '0';
                    vm.datosCDPGR.push(cdpRP);

                });
                window.setTimeout(function () {
                    $(window).resize();
                    $(window).resize();
                }, 10);

            });

        }

        function archivoSeleccionado() {
            let file = $scope.files[0].arhivo; //evt.target.files[0]; //$scope.files; //
            if ($scope.validaNombreArchivo(file.name)) {
                if (typeof (FileReader) != "undefined") {
                    var reader = new FileReader();
                    //For Browsers other than IE.
                    if (reader.readAsBinaryString) {
                        reader.onload = function (e) {
                            $scope.ProcessExcel(e.target.result);
                        };
                        reader.readAsBinaryString(file);
                    } else {
                        //For IE Browser.
                        reader.onload = function (e) {
                            var data = "";
                            var bytes = new Uint8Array(e.target.result);
                            for (var i = 0; i < bytes.byteLength; i++) {
                                data += String.fromCharCode(bytes[i]);
                            }
                            $scope.ProcessExcel(data);
                        };
                        reader.readAsArrayBuffer(file);
                    }
                } else {
                    $window.alert(".");
                }
            }

        }

        function limpiarArchivo() {
            $scope.files = [];
            document.getElementById("control").value = "";
            $timeout(function () {
                $scope.data.file = undefined;
            }, 100);
        }

        function validaTabla() {
            var mensajeError = '';
            var valorsuma = 0;
            var valorcdp = 0;
            var valortotalcdp = 0;
            var valormontoproyecto = parseInt(limpiaNumero(vm.ValorMontoProyecto));
            vm.datosCDPGR.map(function (item) {

                if (mensajeError === '') {

                    //var tipo = vm.listaTipoRequisitos.filter(x => x.TipoRequisito === item.tipo);
                    //if (tipo.length < 1)
                    //    mensajeError = agregarMensaje(mensajeError, "El tipo de requisito " + item.tipo + " no es valido!");

                    if (item.numeroCDP < 1)
                        mensajeError = agregarMensaje(mensajeError, "El numero del CDP  " + item.numeroCDP + " no es valido!");

                    if (ValidaSiEsNumero(item.valorCDP)) {
                        valorcdp = parseInt(limpiaNumero(item.valorCDP));
                        valorsuma += valorcdp;
                    }
                    else
                        mensajeError = agregarMensaje(mensajeError, "El valor de aporta del CDP   " + item.numeroCDP + " no es númerico!");

                    if (ValidaSiEsNumero(item.valortotalCDP)) {
                        valortotalcdp = parseInt(limpiaNumero(item.valortotalCDP));
                    }
                    else
                        mensajeError = agregarMensaje(mensajeError, "El valor total del CDP    " + item.numeroCDP + " no es númerico!");

                    if (valorcdp > valortotalcdp)
                        mensajeError = agregarMensaje(mensajeError, "El valor aporta del CDP  " + item.numeroCDP + " es mayor al valor total del CDP!");

                    if (item.descripcion === undefined || item.descripcion === '')
                        mensajeError = agregarMensaje(mensajeError, "La descripción del  CDP no puede estar vacia!");

                    if (item.unidadejecutora === undefined || item.unidadejecutora === '')
                        mensajeError = agregarMensaje(mensajeError, "La unidad ejecutora del  CDP no puede estar vacia!");
                }
                else return true;;

            });
            if (mensajeError !== '') {
                utilidades.mensajeError(mensajeError);
                return false;
            }


            if (mensajeError === '') {
                var lista = vm.datosCDPGR;
                var contadortipos = [];
                lista.map((tipo) => {
                    contadortipos[tipo.numeroCDP] = (contadortipos[tipo.numeroCDP] || 0) + 1;
                }, {});
                contadortipos.map(function (item, index) {
                    if (item > 1) {
                        utilidades.mensajeError("El número CDP " + index + " está repetido en la tabla!");
                        return false;
                    }
                });

            }

            if (mensajeError === '') {
                if (valorsuma !== valormontoproyecto) {
                    utilidades.mensajeError("La suma total del valor aporta es diferente al valor monto del proyecto!");
                    resultado = false;
                }
                else
                    return true;
            }
            else
                return true;

        }

        function validaArchivoCDP(lista) {
            var resultado = true;
            var valorsuma = 0;
            var valorcdp = 0;
            var valortotalcdp = 0;
            var valormontoproyecto = parseInt(limpiaNumero(vm.ValorMontoProyecto));
            lista.map(function (item, index) {
                //var tipo = vm.listaTipoRequisitos.filter(x => x.TipoRequisito === item.Tipo);

                //if (tipo.length === 0) {
                //    utilidades.mensajeError("El tipo de recurso " + item.Tipo + " no existe!");
                //    resultado = false;
                //}

                if (ValidaSiEsNumero(item["Valor CDP"])) {
                    valorcdp = parseInt(limpiaNumero(item["Valor CDP"]));
                    valorsuma += valorcdp;
                }
                else {
                    utilidades.mensajeError("El valor de aporta   " + item["Valor CDP"] + " no es númerico!");
                    resultado = false;
                }
                if (ValidaSiEsNumero(item["Valor Total CDP"])) {
                    valortotalcdp = parseInt(limpiaNumero(item["Valor Total CDP"]));
                }
                else {
                    utilidades.mensajeError("El valor total del CDP    " + item["Valor Total CDP"] + " no es númerico!");
                    resultado = false;
                }
                if (valortotalcdp < valorcdp) {
                    utilidades.mensajeError("El valor aporta del CDP  " + item["Número CDP"] + " es mayor al valor total del CDP!");
                    resultado = false;
                }

                if (!ValidaSiEsNumero(item["Valor Total CDP"])) {
                    utilidades.mensajeError("El valor total del  CDP  " + item["Valor Total CDP"] + " no es númerico!");
                    resultado = false;
                }
               


            });
            if (resultado) {
                vm.datosCDPGR.map(function (item, index) {
                    valorsuma += parseInt(limpiaNumero(item.valorCDP));
                });
                if (valorsuma > valormontoproyecto) {
                    utilidades.mensajeError("La suma total del valor aporta del arvhivo y de la tabla es mayor al valor monto del proyecto!");
                    resultado = false;
                }

            }
            if (resultado) {
                var contadortipos = [];
                lista.map((tipo) => {
                    contadortipos[tipo["Número CDP"]] = (contadortipos[tipo["Número CDP"]] || 0) + 1;
                }, {});
                contadortipos.map(function (item, index) {
                    if (item > 1) {
                        utilidades.mensajeError("El número CDP " + index + " está repetido en el archivo!");
                        resultado = false;
                    }
                });
            }
            if (resultado) {
                lista.map(function (item, index) {
                    var cdp = vm.datosCDPGR.filter(x => x.numeroCDP === item["Número CDP"]);
                    if (cdp.length > 0) {
                        utilidades.mensajeError("El número del CDP  " + item["Número CDP"] + " ya existe en la tabla!");
                        resultado = false;
                    }
                });
            }
            return resultado;
        }

        function exportExcel() {
            const filename = 'Template_.xlsx';
            const COL_PARAMS = ['hidden', 'wpx', 'width', 'wch', 'MDW'];
            const STYLE_PARAMS = ['fill', 'font', 'alignment', 'border'];
            var styleConf = {
                'E4': {
                    fill: { fgColor: { rgb: 'FFFF0000' } }
                }
            }

            var columns = [
                {
                    name: 'numeroCDP', title: 'Número CDP'
                },
                {
                    name: 'valorCDP', title: 'Valor CDP Trámite'
                },
                {
                    name: 'fechaCDP', title: 'Fecha CDP'
                },
                {
                    name: 'descripcion', title: 'Descripción CDP'
                },
                {
                    name: 'valortotalCDP', title: 'Valor Total CDP Trámite'
                },
                {
                    name: 'unidadejecutora', title: 'Unidad Ejecutora'
                },
            ];

            let colNames = columns.map(function (item) {
                return item.title;
            })

            var wb = XLSX.utils.book_new();

            wb.Props = {
                Title: "Plantilla tramite CDP",
                Subject: "PIIP",
                Author: "PIIP",
                CreatedDate: new Date().getDate()
            };

            wb.SheetNames.push("Hoja Plantilla");

            const header = colNames;
            const data = [{
                "Número CDP": "122.021",
                "Valor CDP": "2.000.000",
                "Fecha CDP": "23/05/2021",
                "Descripcion": "texto",
                "Valor Total CDP": "6.500.000",
                "Unidad Ejecutora": "texto"
            }];

            const worksheet = XLSX.utils.json_to_sheet(data, { header: ["Número CDP", "Valor CDP", "Fecha CDP", "Descripcion", "Valor Total CDP", "Unidad Ejecutora"] });
            for (let col of [1, 2, 3, 4, 5, 6]) {
                formatColumn(worksheet, col, "#.###")
            }


            wb.Sheets["Hoja Plantilla"] = worksheet;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaTramiteCDP.xlsx');

        }

        function formatColumn(worksheet, col) {
            var fmtnumero = "#.###";
            var fmtfecha = "dd/MM/yyyy";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })

                if (worksheet[ref] && worksheet[ref].t === 'n') {
                    if (ref === "A2" || ref === "B2" || ref === "D2") {
                        worksheet[ref].z = fmtnumero;
                        worksheet[ref].t = 'n';
                    }

                    else if (ref === "C2")
                        worksheet[ref].z = fmtfecha
                }
            }
        }

        function s2ab(s) {
            var buf = new ArrayBuffer(s.length); //convert s to arrayBuffer
            var view = new Uint8Array(buf);  //create uint8array as viewer
            for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF; //convert to octet
            return buf;
        }

        $scope.SelectFile = function (event) {

            $scope.files = [];
            var files = [];


            if (event != null) {
                event.preventDefault();
            }
            files.push(event.currentTarget.files);


            files.forEach(function (item) {
                var reader = new FileReader();
                reader.readAsDataURL(item[0]);
                if ($scope.validaNombreArchivo(item[0].name))
                    $scope.files.push({ nombreArchivo: item[0].name, size: item[0].size, arhivo: item[0] });

            });
        };

        $scope.ProcessExcel = function (data) {
            //var data = $scope.files;
            var workbook = XLSX.read(data, {
                type: 'binary'
            });
            var firstSheet = workbook.SheetNames[0];

            var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);

            if (validaArchivoCDP(excelRows)) {
                $scope.$apply(function () {
                    try {
                        //"Número CDP", "Valor CDP", "Fecha CDP", "Descripcion", "Valor Total CDP", "Unidad Ejecutora"
                        excelRows.map(function (item, index) {
                            //var tipo = vm.listaTipoRequisitos.filter(x => x.TipoRequisito === item.Tipo);
                            var fecha = new Date();
                            if (item["Fecha CDP"].length === 10) {
                                var from = item["Fecha CDP"].split("/");
                                fecha = new Date(from[2], from[1] - 1, from[0]);
                            }
                            cdpRP = {
                                idproyectotramite: 0,
                                idproyectoRequisitotramite: 0,
                                idtiporequito: 1,
                                idpresupuestovaloresCDP: 0,
                                idpresupuestovaloresaportaCDP: 0,
                                tipo: "CDP",
                                numeroCDP: item["Número CDP"],
                                valorCDP: formatearNumero(limpiaNumero(item["Valor CDP"])),
                                fechaCDP: fecha,
                                descripcion: item["Descripcion"],
                                valortotalCDP: formatearNumero(limpiaNumero(item["Valor Total CDP"])),
                                unidadejecutora: item["Unidad Ejecutora"]
                            }
                            vm.datosCDPGR.push(cdpRP);
                            window.setTimeout(function () {
                                $(window).resize();
                                $(window).resize();
                            }, 100);
                        });
                        limpiarArchivo();
                    }
                    catch (ex) {
                        utilidades.mensajeError("Debe validar que el archivo corresponda a la plantilla!");
                    }

                });
            }
        };

        $scope.validaNombreArchivo = function (nombre) {
            var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
            if (!regex.test(nombre.toLowerCase())) {
                utilidades.mensajeError("El archivo no es de tipo Excel!");
                $scope.files = [];
                $scope.nombreArchivo = '';
                return false;
            }
            else {
                return true;
            }
        }


        /*funciones generales*/

        function ValidaSiEsNumero(valor) {
            if (valor === undefined)
                return false;
            else if (!isNaN(limpiaNumero(valor))) {
                return true;
            }
            else {
                return false;
            }

        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        function cambiarComasPorPunto(valor) {
            return valor.toString().replaceAll(",", ".");
        }

        function formatearNumero(value) {
            var numerotmp = value.toString().replaceAll('.', '');
            return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        }

        function formatoFecha(value) {
            value = new Date(value);
            var dia = value.getDate() + "";
            var mes = (value.getMonth() + 1) + "";
            var ano = value.getFullYear() + "";
            return ano + "-" + agregarCero(mes) + "-" + agregarCero(dia);
        }

        function agregarCero(value) {
            if (value.length == 1) {
                value = "0" + value;
            }
            return value;

        }

        function agregarMensaje(mensaje, value) {
            if (mensaje === '')
                return value;
            else
                return mensaje += ', ' + value;
        }

    }

    angular.module('backbone').component('cdpTramite', {
        templateUrl: "src/app/formulario/ventanas/tramites/componentes/cdp/cdpTramite.html",
        controller: cdpTramiteController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    }).directive("fileDropzone", function () {
        return {
            restrict: 'A',
            scope: {
                filesToUpload: '=',
                validaNombreArchivo: '&',
                data: '='
            },
            link: function (scope, element, attrs) {
                var processDragOverOrEnter;
                processDragOverOrEnter = function (event) {
                    if (event != null) {
                        event.preventDefault();
                    }
                    return false;
                };

                var regex = /^([\ \(a-zA-Z0-9\s_\\.\-:\ \)])+(.xls|.xlsx)$/;
                scope.data = '';
                scope.filesToUpload = [];
                element.bind('dragover', processDragOverOrEnter);
                element.bind('dragenter', processDragOverOrEnter);

                return element.bind('drop', function (event) {
                    // try {
                    scope.filesToUpload = (event);
                    var files = [];
                    scope.filesToUpload = [];

                    if (event != null) {
                        event.preventDefault();
                    }

                    var fileCount = 0;
                    angular.forEach(event.originalEvent.dataTransfer.files, function (item) {
                        if (fileCount < 10) { //Can add a variety of file validations                              
                            files.push(item);
                        }
                        fileCount++;
                    });
                    if (fileCount > 10) alert("You can only select up to 10 files. Please note only the first 10 will be processed.");


                    files.forEach(function (item) {



                        var reader = new FileReader();

                        reader.readAsDataURL(item);
                        scope.data = item.name;
                        scope.filesToUpload.push({ nombreArchivo: item.name, size: item.size, arhivo: item });
                        scope.validaNombreArchivo()(scope.data);

                    });
                });


            },
            controller: function ($scope, $element) {
            }

        }
    });

})();