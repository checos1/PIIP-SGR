
(function () {
    'use strict';
    angular.module('backbone').factory('cargaMasivaCdpService', cargaMasivaCdpService);

    cargaMasivaCdpService.$inject = ['$q', '$location', 'constantesBackbone'];

    function cargaMasivaCdpService($q, $location, constantesBackbone) {
        return {
            generarDocumentoEjemploCdp: generarDocumentoEjemplo,
			procesarArchivoCdp: procesarArchivoCdp,
			validarArchivo: validarArchivo,
			convertirRespuestaAModelo: convertirRespuestaAModelo
        };

        function generarDocumentoEjemplo() {
            const dataEjemplo = [{
                "Número": "122021",
                "Fecha": "23/05/2021",
                "Valor total": "2000004",
                "Valor CDP para tramite": "2000002"
            }];

            const hojaEjemplo = XLSX.utils.json_to_sheet(dataEjemplo, { dateNF: 'YYYY/MM/DD HH:mm:ss', cellDates: true });
            var libro = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(libro, hojaEjemplo, "CDP");
            XLSX.writeFile(libro, "Cdp ejemplo.xlsx");
		}

		function validarArchivo(arrayData, existingCdps) {
			const errorList = [];

			if (arrayData.cdpList !== undefined && arrayData.cdpList.length > 0) {
				let cdpValues = JSON.stringify(Object.keys(arrayData.cdpList[0]));
				if (JSON.stringify(['Número', 'Fecha', 'Valor total', 'Valor CDP para tramite']) !== cdpValues) {
					isValid = false;
					errorList.push('columnas del archivo no coinciden, hoja precontractual');
				} else {
					arrayData.cdpList.forEach((cdp) => {
						const isRepeatinExisting = existingCdps.find(w => w.NumeroCDP == cdp['Número']);
						if (isRepeatinExisting !== undefined) {
							errorList.push(`El CDP con número ${cdp['Número']} ya se encuentra registrado para el proyecto actual`);
						}

						const valorTotal = Number(limpiaNumero(cdp['Valor total']));;
						const valorCdp = Number(limpiaNumero(cdp['Valor CDP para tramite']));

						if (valorTotal=== isNaN) {
							errorList.push(`El valor total del CDP del CDP con número ${cdp['Número']} es incorrecto`);
						}

						if (valorCdp === isNaN) {
							errorList.push(`El valor CDP para trámite del CDP con número ${cdp['Número']} es incorrecto`);
						}

						if (valorCdp !== isNaN && valorTotal !== isNaN && valorCdp > valorTotal) {
							errorList.push(`los valores del CDP con número ${cdp['Número']} son incorrectos, El valor CDP para el trámite debe ser menor al valor total CDP`);
						}
					})
                }
			} else {
				errorList.push(`El archivo cargado está vacío`);
            }

			return {
				isValid: errorList.length == 0,
				errorList: errorList
            }
        }

        function procesarArchivoCdp(file) {
			return new Promise((resolve, reject) => {
				try {
					let arrayBuffer;
					if (file) {
						const fileReader = new FileReader();
						fileReader.readAsArrayBuffer(file);
						fileReader.onload = (e) => {
							arrayBuffer = fileReader.result;
							const data = new Uint8Array(arrayBuffer);
							const arr = new Array();
							for (let i = 0; i != data.length; ++i) {
								arr[i] = String.fromCharCode(data[i]);
							}
							const bstr = arr.join('');
							const workbook = XLSX.read(bstr, { type: 'binary', cellText: false, cellDates: true });
							var cdpList = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { raw: false, cellText: false, cellDates: true });

							if (workbook.SheetNames[0] !== 'CDP') {
								reject('El archivo cargado no tiene la estructura requerida');
							} else {
								const sheets = { cdpList };
								resolve(sheets);
							}
						};
					} else {
						var datosArchivo = null;
						resolve(null)
					}
				} catch (error) {
					reject(error);
				}
			});
		}

		function convertirRespuestaAModelo(arrayDataCdp, tramiteId, proyectoId, rolId) {
			return arrayDataCdp.cdpList.map((cdp) => {
				return {
					Descripcion: 'CDP para trámite',
					FechaCDP: moment(cdp['Fecha'], 'DD/MM/YYYY').toDate(),
					IdPresupuestoValoresCDP: 0,
					IdPresupuestoValoresAportaCDP: 0,
					IdProyectoRequisitoTramite: 0,
					IdProyectoTramite: 0,
					IdTipoRequisito: 1,
					NumeroCDP: limpiaNumero(cdp['Número']),
					NumeroContratoCDP: 0,
					Tipo: 1,
					UnidadEjecutora: '',
					ValorTotalCDP: limpiaNumero(cdp['Valor total']),
					ValorCDP: limpiaNumero(cdp['Valor CDP para tramite']),
					IdValorTotalCDP: 0,
					IdValorAportaCDP: 0,
					IdProyecto: proyectoId,
					IdTramite: tramiteId,
					IdTipoRol: 0,
					IdRol: rolId
				}
			});
		}

		function limpiaNumero(valor) {
			return valor.toString().replaceAll(".", "");
		}


    }
})();