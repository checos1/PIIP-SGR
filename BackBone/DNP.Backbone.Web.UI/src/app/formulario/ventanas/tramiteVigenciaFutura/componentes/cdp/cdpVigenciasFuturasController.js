(function () {
    'use strict';

    cdpVigenciasFuturasController.$inject = ['$scope', 'gestionRecursosServicio', '$sessionStorage', '$uibModal', 'utilidades', 'tramiteVigenciaFuturaServicio',
        'justificacionCambiosServicio', 'constantesBackbone'];

    function cdpVigenciasFuturasController($scope,
        gestionRecursosServicio,
        $sessionStorage,
        $uibModal,
        utilidades,
        tramiteVigenciaFuturaServicio,
        justificacionCambiosServicio,
        constantesBackbone,
    ) {
        var vm = this;
        vm.init = init;
        vm.nombreComponente = "informacionpresupuestalcdp";
        vm.abrirModalEditarCDP = abrirModalEditarCDP;
        vm.BPIN = $sessionStorage.idObjetoNegocio;
        vm.eliminarCDP = eliminarCDP;
        vm.editarCDP = editarCDP;
        vm.informacionCDP = informacionCDP;
        vm.disabled = true;
        vm.TotalInversionVigencia = 0;
        vm.notificacionErrores = null;
        vm.erroresActivos = [];
        vm.exportExcel = exportExcel;
        vm.ProyectoId = $sessionStorage.ProyectoId;
        vm.TramiteId = $sessionStorage.TramiteId;
        vm.TipoRolId = 0;
        vm.IdInstancia = $sessionStorage.idInstancia;
        vm.idFlujo = $sessionStorage.idFlujoIframe;
        $scope.files = [];
        vm.limpiarArchivo = limpiarArchivo;
        vm.archivoSeleccionado = archivoSeleccionado;
        $sessionStorage.listaCDP = [];
        $sessionStorage.esAjuste = false;
        vm.idRol = '';
        vm.idsroles = [];
        vm.parametrosObjetosNegocioDto = {};
        vm.listaCDP = [];

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        function init() {

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.parametrosObjetosNegocioDto.EntidadId = $sessionStorage.idEntidad;
            vm.parametrosObjetosNegocioDto.IdUsuarioDNP = $sessionStorage.usuario.permisos.IdUsuarioDNP;
            vm.parametrosObjetosNegocioDto.UsuarioDNP = '';
            $sessionStorage.usuario.roles.map(function (item) {
                vm.idsroles.push(item.IdRol);
            });
            vm.parametrosObjetosNegocioDto.IdsRoles = vm.idsroles;
            tramiteVigenciaFuturaServicio.obtenerInstanciasPermiso(vm.parametrosObjetosNegocioDto).then(function (result) {
                result.data.map(function (item) {
                    if (item.InstanciaId === vm.IdInstancia && item.FlujoId === vm.idFlujo) {
                        vm.idsroles.map(function (itemRol) {
                            if (itemRol === item.RolId)
                                vm.idRol = item.RolId;
                        });
                    }
                });
            });

            vm.obtenerFuentes(vm.TramiteId, vm.ProyectoId);
            vm.esAjuste = $sessionStorage.esAjuste;

            $scope.$watch(function () {
                if (vm.TramiteId != $sessionStorage.tramiteId || vm.ProyectoId != $sessionStorage.proyectoId) {
                    vm.TramiteId = $sessionStorage.tramiteId;
                    vm.ProyectoId = $sessionStorage.proyectoId;
                    vm.obtenerFuentes(vm.TramiteId, vm.ProyectoId);
                }
                return $sessionStorage;
            }, function (newVal, oldVal) {

            }, true);

        }

        //vm.notificacionValidacionHijo = function (handler) {
        //    vm.notificacionErrores = handler;
        //}

        vm.obtenerFuentes = function (tramiteId, proyectoId) {
            if (tramiteId != undefined && proyectoId != undefined) {
                return tramiteVigenciaFuturaServicio.obtenerProyectoRequisitosPorTramite(proyectoId, tramiteId, true)
                    .then(function (result) {
                        var listaCDP = [];

                        if (result.data.length > 0)

                            if (result.data !== undefined && result.data.length > 0) {
                                result.data.map(function (item, index) {

                                    var cdpRP = {}
                                    cdpRP.id = item.Id
                                    cdpRP.descripcion = item.Descripcion
                                    cdpRP.numeroCDP = item.Numero;
                                    cdpRP.tipoRequisitoId = item.TipoRequisitoId;
                                    cdpRP.tramiteProyectoId = item.TramiteProyectoId;
                                    cdpRP.unidadEjecutora = item.UnidadEjecutora;
                                    cdpRP.fechaCDPOriginal = new Date(item.Fecha);
                                    cdpRP.fechaCDP = formatDate(new Date(item.Fecha));
                                    if (item.ListaTiposRequisito !== undefined && item.ListaTiposRequisito.length > 0) {
                                        item.ListaTiposRequisito.map(function (itemfuente, indexfuente) {
                                            cdpRP.tipo = "CDP";
                                            if (itemfuente.ListaValores !== undefined && itemfuente.ListaValores.length > 0) {
                                                itemfuente.ListaValores.map(function (itemvalor, indexvalor) {
                                                    if (itemvalor.TipoValor.TipoValorFuente === 'Valor Aporta') {
                                                        cdpRP.valorCDP = ConvertirNumero(itemvalor.Valor);
                                                        cdpRP.idvaloraportaCDP = itemvalor.TipoValor.Id;
                                                    }
                                                    else if (itemvalor.TipoValor.TipoValorFuente === 'Valor') {
                                                        cdpRP.valortotalCDP = ConvertirNumero(itemvalor.Valor);
                                                        cdpRP.idvalorCDP = itemvalor.TipoValor.Id;
                                                    }
                                                });
                                            }

                                        });

                                    }

                                    listaCDP.push(cdpRP);
                                });
                            }

                        vm.listaCDP = listaCDP;

                        var listaCDPParaVentanaModal = []

                        vm.listaCDP.map(function (item) {

                            var tramiterequisito = {
                                Descripcion: item.descripcion,
                                FechaCDP: item.fechaCDPOriginal,
                                IdPresupuestoValoresCDP: 0,
                                IdPresupuestoValoresAportaCDP: 0,
                                IdProyectoRequisitoTramite: item.id,
                                IdProyectoTramite: item.tramiteProyectoId,
                                IdTipoRequisito: 1,
                                NumeroCDP: item.numeroCDP,
                                Tipo: 'CDP',
                                UnidadEjecutora: item.unidadEjecutora,
                                ValorCDP: limpiaNumero(item.valorCDP),
                                ValorTotalCDP: limpiaNumero(item.valortotalCDP),
                                IdValorTotalCDP: 0,
                                IdValorAportaCDP: 0,
                                IdProyecto: proyectoId,
                                IdTramite: tramiteId,
                                IdTipoRol: vm.TipoRolId,
                                IdRol: vm.idRol
                            }

                            listaCDPParaVentanaModal.push(tramiterequisito)
                        });

                        $sessionStorage.listaCDP = listaCDPParaVentanaModal

                        //para guardar los capitulos modificados y que se llenen las lunas
                        if (listaCDPParaVentanaModal.length == 0) {
                            eliminarCapitulosModificados();
                        }
                        else {
                            guardarCapituloModificado();
                        }
                    });
            }
        }

        function editarCDP(cdpId) {
            $sessionStorage.esAjuste = true;
            var listaCDPActualizada = $sessionStorage.listaCDP.filter(x => x.IdProyectoRequisitoTramite === cdpId);
            $sessionStorage.numeroCDPEditar = listaCDPActualizada[0].NumeroCDP;
            $sessionStorage.fechaEditar = listaCDPActualizada[0].FechaCDP;
            $sessionStorage.valorTotalEditar = parseFloat(listaCDPActualizada[0].ValorCDP.replaceAll(',', '.'));
            $sessionStorage.valorCDPTramiteEditar = parseFloat(listaCDPActualizada[0].ValorTotalCDP.replaceAll(',', '.'));

            abrirModalEditarCDP();


        }

        function abrirModalEditarCDP() {
            const span = document.getElementById('id-capitulo-informacionpresupuestalcdp');
            $sessionStorage.seccionCapitulo = span.textContent;
            $uibModal.open({
                templateUrl: 'src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/cdp/modalEditarCdp.html',
                controller: 'modalEditarCdpController',

            }).result.then(function (result) {
                init();
                $sessionStorage.esAjuste = false;

            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idAplicacion");
            };
        }

        function eliminarCDP(cdpId) {

            utilidades.mensajeWarning("Esto eliminará la información correspondiente Al CDP. ¿Esta seguro de continuar?",
                function funcionContinuar() {
                    var listaCDPActualizada = $sessionStorage.listaCDP.filter(x => x.IdProyectoRequisitoTramite != cdpId);

                    if (listaCDPActualizada.length == 0) {
                        $sessionStorage.listaCDP.map(a => a.Descripcion = "BorrarTodo");
                        listaCDPActualizada = $sessionStorage.listaCDP;
                    }

                    listaCDPActualizada = listaCDPActualizada.map(a => {
                        a.ValorCDP = a.ValorCDP.toString().replaceAll(',', '.');
                        a.ValorTotalCDP = a.ValorTotalCDP.toString().replaceAll(',', '.');
                        return a;
                    });

                    return tramiteVigenciaFuturaServicio.actualizarTramitesRequisitos(listaCDPActualizada)
                        .then(function (response) {
                            if (response.status == "200") {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                vm.obtenerFuentes(vm.TramiteId, vm.ProyectoId);


                            }
                            else {
                                utilidades.mensajeError("Error al realizar la operación", false);
                            }
                        })
                        .catch(error => {
                            if (error.status == "400") {
                                utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                return;
                            }
                            utilidades.mensajeError("Error al realizar la operación");
                        });
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El CDP será eliminado."
            )
        }

        function informacionCDP() {
            return utilidades.mensajeInformacion("Esta es la explicación de carga masiva... un hecho establecido hace demasiado tiempo que un lector se distraerá con el contenido del texto de un sitio mientras que mira su diseño. El punto de usr Lorem ipsum es que tiene una distribución más o menos normal de las letras, al contrario de usar textos como por ejemplo ''Contenido aqui, contenido aqui''.", false, "Archivo plantilla carga masiva");

        }

        vm.onKeyPress_st = function (e) {
            const charCode = e.which ? e.which : e.keyCode;

            if (charCode !== 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
            }

            if (charCode == 13) {
                e.preventDefault();
            }
        }

        function limpiaNumero(valor) {
            return valor.toLocaleString().replaceAll(".", "");
        }

        function archivoSeleccionado() {

            var mensaje = "";
            let file = $scope.files[0].arhivo;

            if ($scope.validaNombreArchivo(file.name)) {
                if (typeof (FileReader) != "undefined") {
                    var reader = new FileReader();
                    if (reader.readAsBinaryString) {
                        reader.onload = function (e) {

                            var workbook = XLSX.read(e.target.result, {
                                type: 'binary'
                            });
                            var firstSheet = workbook.SheetNames[0];
                            var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
                            excelRows.map(function (item, index) {

                                if (item["Número"] == undefined) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": La columna ''Número'' no trae valor. ");
                                }

                                var valorCDPparaTramite = stringtoNumber(item["Valor CDP para tramite"]);
                                if (!(!isNaN(valorCDPparaTramite) && isFinite(valorCDPparaTramite))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Valor CDP para tramite''   " + item["Valor CDP para tramite"] + " no es númerico. ");
                                }

                                var valorTotal = stringtoNumber(item["Valor total"]);
                                if (!(!isNaN(valorTotal) && isFinite(valorTotal))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Valor total''    " + item["Valor total"] + " no es númerico. ");
                                }

                                var date = stingToDate_(item["Fecha"]);
                                if (!(date instanceof Date && !isNaN(date))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": ''Fecha no Válida'' " + item["Fecha"] + " ");
                                }
                            });

                            if (mensaje == "") {
                                var contadortipos = [];
                                excelRows.map((tipo) => {
                                    contadortipos[tipo["Número"]] = (contadortipos[tipo["Número"]] || 0) + 1;
                                }, {});
                                contadortipos.map(function (item, index) {
                                    if (item > 1) {
                                        mensaje = mensaje.concat("Fila" + (item + 1) + ": El ''Número'' " + index + " está repetido en el archivo. ");
                                    }
                                });
                            }
                            if (mensaje == "") {
                                excelRows.map(function (item, index) {
                                    if (vm.listaCDP != undefined) {
                                        var cdp = vm.listaCDP.filter(x => x.numeroCDP == item["Número"]);
                                        if (cdp.length > 0) {
                                            mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Número''  " + item["Número"] + " ya existe en la tabla. ");
                                        }
                                    }
                                });
                            }

                            if (mensaje != "") {
                                utilidades.mensajeError("El archivo adjuntado debe ser exactamente la plantilla para carga masiva, contener las celdas que allí se diseñaron, y estar diligenciado. Errores encontrados: " + mensaje, false, "La validación no cumple con los parámetros.");
                                return limpiarArchivo();
                            }

                            $scope.ProcessExcel(e.target.result);
                        };
                        reader.readAsBinaryString(file);
                    } else {
                        reader.onload = function (e) {
                            var data = "";
                            var bytes = new Uint8Array(e.target.result);
                            for (var i = 0; i < bytes.byteLength; i++) {
                                data += String.fromCharCode(bytes[i]);
                            }

                            var workbook = XLSX.read(e.target.result, {
                                type: 'binary'
                            });
                            var firstSheet = workbook.SheetNames[0];
                            var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);
                            excelRows.map(function (item, index) {

                                if (item["Número"] == undefined) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": La columna ''Número'' no trae valor. ");
                                }

                                var valorCDPparaTramite = stringtoNumber(item["Valor CDP para tramite"]);
                                if (!(!isNaN(valorCDPparaTramite) && isFinite(valorCDPparaTramite))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Valor CDP para tramite''   " + item["Valor CDP para tramite"] + " no es númerico. ");
                                }

                                var valorTotal = stringtoNumber(item["Valor total"]);
                                if (!(!isNaN(valorTotal) && isFinite(valorTotal))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Valor total''    " + item["Valor total"] + " no es númerico. ");
                                }

                                var date = stingToDate_(item["Fecha"]);
                                if (!(date instanceof Date && !isNaN(date))) {
                                    mensaje = mensaje.concat("Fila" + (index + 1) + ": ''Fecha no Válida'' " + item["Fecha"] + " ");
                                }
                            });

                            if (mensaje == "") {
                                var contadortipos = [];
                                excelRows.map((tipo) => {
                                    contadortipos[tipo["Número"]] = (contadortipos[tipo["Número"]] || 0) + 1;
                                }, {});
                                contadortipos.map(function (item, index) {
                                    if (item > 1) {
                                        mensaje = mensaje.concat("Fila" + (item + 1) + ": El ''Número'' " + index + " está repetido en el archivo. ");
                                    }
                                });
                            }
                            if (mensaje == "") {
                                excelRows.map(function (item, index) {
                                    if (vm.listaCDP != undefined) {
                                        var cdp = vm.listaCDP.filter(x => x.numeroCDP == item["Número"]);
                                        if (cdp.length > 0) {
                                            mensaje = mensaje.concat("Fila" + (index + 1) + ": El ''Número''  " + item["Número"] + " ya existe en la tabla. ");
                                        }
                                    }
                                });
                            }

                            if (mensaje != "") {
                                utilidades.mensajeError("El archivo adjuntado debe ser exactamente la plantilla para carga masiva, contener las celdas que allí se diseñaron, y estar diligenciado. Errores encontrados: " + mensaje, false, "La validación no cumple con los parámetros.");
                                return limpiarArchivo();
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

        $scope.ProcessExcel = function (data) {
            var workbook = XLSX.read(data, {
                type: 'binary'
            });
            var firstSheet = workbook.SheetNames[0];

            var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);

            $scope.$apply(function () {
                try {

                    var tramiteRequisitosDto = [];
                    tramiteRequisitosDto = $sessionStorage.listaCDP;
                    excelRows.map(function (item, index) {
                        var tramiterequisito = {
                            Descripcion: 'CDP para trámite VFO',
                            FechaCDP: stingToDate_(item["Fecha"]),
                            IdPresupuestoValoresCDP: 0,
                            IdPresupuestoValoresAportaCDP: 0,
                            IdProyectoRequisitoTramite: 0,
                            IdProyectoTramite: 0,
                            IdTipoRequisito: 1,
                            NumeroCDP: item["Número"],
                            Tipo: 'CDP',
                            UnidadEjecutora: '',
                            ValorCDP: stringtoNumber(item["Valor total"]),
                            ValorTotalCDP: stringtoNumber(item["Valor CDP para tramite"]),
                            IdValorTotalCDP: 0,
                            IdValorAportaCDP: 0,
                            IdProyecto: vm.ProyectoId,
                            IdTramite: vm.TramiteId,
                            IdTipoRol: vm.TipoRolId,
                            IdRol: vm.idRol
                        }
                        tramiteRequisitosDto.push(tramiterequisito);
                    });

                    tramiteRequisitosDto = tramiteRequisitosDto.map(a => {
                        a.ValorCDP = a.ValorCDP.toString().replaceAll(',', '.');
                        a.ValorTotalCDP = a.ValorTotalCDP.toString().replaceAll(',', '.');
                        return a;
                    });

                    tramiteVigenciaFuturaServicio.actualizarTramitesRequisitos(tramiteRequisitosDto)
                        .then(function (response) {
                            if (response.status == "200") {
                                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                vm.obtenerFuentes(vm.TramiteId, vm.ProyectoId);
                            }
                            else {
                                utilidades.mensajeError("Error al realizar la operación", false);
                            }
                        })
                        .catch(error => {
                            if (error.status == 400) {
                                utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                return;
                            }
                            utilidades.mensajeError("Error al realizar la operación");
                        });
                }
                catch (ex) {
                    utilidades.mensajeError("Debe validar que el archivo corresponda a la plantilla!");
                }
            });

            limpiarArchivo();

        };

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

        function exportExcel() {
            var columns = [
                {
                    name: 'Numero', title: 'Número'
                },
                {
                    name: 'Fecha', title: 'Fecha'
                },
                {
                    name: 'Valor total', title: 'Valor total'
                },
                {
                    name: 'Valor CDP para tramite', title: 'Valor CDP para trámite'
                }
            ];

            let colNames = columns.map(function (item) {
                return item.title;
            })

            var wb = XLSX.utils.book_new();

            wb.Props = {
                Title: "Plantilla CDP",
                Subject: "PIIP",
                Author: "PIIP",
                CreatedDate: new Date().getDate()
            };

            wb.SheetNames.push("Hoja Plantilla");

            const header = colNames;
            const data = [{
                "Número": "122021",
                "Fecha": "23/05/2021",
                "Valor total": "2000000.56",
                "Valor CDP para tramite": "2000000.56"
            }];

            const worksheet = XLSX.utils.json_to_sheet(data, colNames);

            for (let col of [1, 2, 3, 4]) {
                formatColumn(worksheet, col, "#.###")
            }

            wb.Sheets["Hoja Plantilla"] = worksheet;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'PlantillaCDP.xlsx');
        }

        function formatColumn(worksheet, col) {
            var fmtnumero = "#.###";
            var fmtfecha = "dd/MM/yyyy";
            const range = XLSX.utils.decode_range(worksheet['!ref'])
            for (let row = range.s.r + 1; row <= range.e.r; ++row) {
                const ref = XLSX.utils.encode_cell({ r: row, c: col })

                if (worksheet[ref] && worksheet[ref].t === 'n') {
                    if (ref === "A2" || ref === "C2" || ref === "D2") {
                        worksheet[ref].z = fmtnumero;
                        worksheet[ref].t = 'n';
                    }

                    else if (ref === "B2")
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

        function stingToDate_(date) {
            if (/^\d+$/.test(date)) {
                return new Date(1899, 12, (parseInt(date) - 1));
            }
            else if (date.includes("/")) {
                if ((date.split("/").length - 1) == 2) {
                    return stringToDate(date, "dd/MM/yyyy", "/");
                }
                else {
                    return NaN;
                }
            }
            else {
                return NaN;
            }
        }

        function stringToDate(_date, _format, _delimiter) {
            var formatLowerCase = _format.toLowerCase();
            var formatItems = formatLowerCase.split(_delimiter);
            var dateItems = _date.split(_delimiter);
            var monthIndex = formatItems.indexOf("mm");
            var dayIndex = formatItems.indexOf("dd");
            var yearIndex = formatItems.indexOf("yyyy");
            var month = parseInt(dateItems[monthIndex]);
            var days = parseInt(dateItems[dayIndex]);
            month -= 1;
            if (month > 12) {
                return NaN;
            }
            if (days > 31) {
                return NaN;
            }
            if (days > 29 && month == 1) {
                return NaN;
            }
            if (days > 30 && (month == 3 || month == 5 || month == 8 || month == 10)) {
                return NaN;
            }
            var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
            return formatedDate;
        }

        function stringtoNumber(number) {
            var withoutComma = number.toString().replace(",", "");
            return new Number(withoutComma);
        }

        function ConvertirNumero(numero) {
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(numero);
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            //vm.seccionCapitulo = $uibModalInstance.seccionCapitulo
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
        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
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
            const span = document.getElementById('id-capitulo-informacionpresupuestalcdp');
            vm.seccionCapitulo = span.textContent;


        }

        /* ------------------------ Validaciones ---------------------------------*/
        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }

                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
                }
            }
        }


        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-CDP-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }
            var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-CDPrecursosNación-error");
            if (campoObligatorioProyectos != undefined) {
                campoObligatorioProyectos.innerHTML = "";
                campoObligatorioProyectos.classList.add('hidden');
            }

            if (vm.listaCDP !== undefined)
                vm.listaCDP.forEach(p => {
                    var campoObligatorioProyectos = document.getElementById(vm.nombreComponente + "-" + p.numeroCDP);
                    if (campoObligatorioProyectos != undefined) {
                        campoObligatorioProyectos.innerHTML = "";
                        campoObligatorioProyectos.classList.add('hidden');
                    }
                }
                );
        }

        vm.validarValores = function (index1, index2, errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + index1 + index2);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionPresupuestalCDP = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-CDP-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionPresupuestalCDPRecursosNAcion = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-CDPrecursosNación-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresVigenciaInformacionPresupuestalCDGrilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById(vm.nombreComponente + "-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'VFO001': vm.validarValoresVigenciaInformacionPresupuestalCDP,
            'VFO001-': vm.validarValoresVigenciaInformacionPresupuestalCDGrilla,
            'VFO002': vm.validarValoresVigenciaInformacionPresupuestalCDPRecursosNAcion,

        }

        function padTo2Digits(num) {
            return num.toString().padStart(2, '0');
        }

        function formatDate(date) {
            return [
                padTo2Digits(date.getDate()),
                padTo2Digits(date.getMonth() + 1),
                date.getFullYear(),
            ].join('/');
        }

        /* ------------------------ FIN Validaciones ---------------------------------*/
    }
    angular
        .module('backbone')
        .component('cdpVigenciasFuturas', {
            templateUrl: "src/app/formulario/ventanas/tramiteVigenciaFutura/componentes/cdp/cdpVigenciasFuturas.html",
            controller: cdpVigenciasFuturasController,
            controllerAs: "vm",
            bindings: {
                callback: '&',
                guardadoevent: '&',
                notificacionvalidacion: '&',
                notificacionestado: '&'
            }
        });
})();
