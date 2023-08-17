function  printByDocumentId(documentId) {
    const ficha = document.getElementById(documentId)
    const encabezado = document.getElementById('printHeader')
    const pie = document.getElementById('pie')
    const ventimp = window.open('', 'popimpr')
    const estilos = obtenerEstilos()
    ventimp.document.write(
        `<html>
        <head>
        <title>Akanuxt</title>
            <style>
        *{font-size: 14px;} 
        ${estilos}   
    
            </style>
        </head>`
    )
    ventimp.document.write('<body>')
    ventimp.document.write('<div class="no-print" style="width:100%; height: 100% ;"></div>')
    ventimp.document.write(`
      <table class="is-fullwidth">        
        <thead>
          <tr>
            <td class="only-print">
              ${encabezado?.innerHTML||""}
            </td>
          </tr>
          
        </thead>
        <tbody>
          <tr>
            <td>
              ${ficha?.innerHTML||""}
            </td>
          </tr>
        </tbody>
        <tfoot style=" height:70px"></tfoot>
      </table>
      <div class="only-print" style="position: fixed;
        width: 100%;
        bottom: 0px;"
        >
       ${pie?.innerHTML||""}
      </div>
             
   
      `)
    ventimp.document.write('</body></html>')
    ventimp.document.close()
    ventimp.print()
    ventimp.close()
}
function  obtenerEstilos() {
    let estilos = ''
    const hojasEstilo = document.styleSheets
    for (let i = 0; i < hojasEstilo.length; i++) {
        const reglas = hojasEstilo[i].cssRules
        if (reglas) {
            for (let j = 0; j < reglas.length; j++) {
                estilos += reglas[j].cssText
            }
        }
    }
    return estilos
}
