    window.renderSampleMermaidDiagram = () => {
        var output = document.getElementById("output");
        var input = "graph LR \n" +
            "A[Client] --- B[Load Balancer] \n" +
            "B-->C[Server01] \n" +
            "B-->D(Server02) \n";
        mermaid.mermaidAPI.render('theGraph', input, function (svgCode) {
            output.innerHTML = svgCode;
        });
    }

window.renderMermaidDiagram = (Id, diagramText) => {

    var output = document.getElementById(Id);

    //var output = document.getElementById("output");
    //mermaid.mermaidAPI.render('theGraph', diagramText, function (svgCode) {
    mermaid.mermaidAPI.render(Id, diagramText, function (svgCode) {
        output.innerHTML = svgCode;
    });
}

function remderMermaid(Id, diagramText){

    var output = document.getElementById(Id);

    //var output = document.getElementById("output");
    //mermaid.mermaidAPI.render('theGraph', diagramText, function (svgCode) {
    mermaid.mermaidAPI.render(Id, diagramText, function (svgCode) {
        output.innerHTML = svgCode;
    });
}

function showAlert(message) {
    alert(message);
}