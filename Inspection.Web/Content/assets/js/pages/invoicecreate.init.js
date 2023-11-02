var productNamesArray = [];

function updateProductNamesArray() {
    productNamesArray = Array.from(document.querySelectorAll(".product input[type='text']")).map(input => input.value);
    console.log(productNamesArray);
    document.getElementById("basiInput").value = productNamesArray.join(', ');
}

function new_link() {
    var count = document.querySelectorAll(".product").length + 1;
    var tr1 = document.createElement("tr");
    tr1.className = "product";

    var delLink = `
        <td class="text-start">
            <input class="form-control" type="text" id="productName-${count}" name="ProductNames[]" placeholder="Enter Rcode" required>
        </td>
        <td class="product-removal">
            <a class="btn btn-danger" onclick="removeRow(${count})">-</a>
        </td>
    `;

    tr1.innerHTML = delLink;
    document.getElementById("newlink").appendChild(tr1);
    updateProductNamesArray();
}

function removeRow(rowId) {
    var rowToRemove = document.querySelector(`#productName-${rowId}`).closest("tr");
    if (rowToRemove) {
        rowToRemove.remove();
        updateProductNamesArray();
    }
}
