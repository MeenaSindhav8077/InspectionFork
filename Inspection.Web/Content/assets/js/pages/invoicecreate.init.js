/*
Template Name: Velzon - Admin & Dashboard Template
Author: Themesbrand
Website: https://Themesbrand.com/
Contact: Themesbrand@gmail.com
File: Invoice create init Js File
*/
var paymentSign = "$";
Array.from(document.getElementsByClassName("product-line-price")).forEach(function (item) {
	item.value = paymentSign + "0.00"
});
function otherPayment() {
	var paymentType = document.getElementById("choices-payment-currency").value;
	paymentSign = paymentType;


	Array.from(document.getElementsByClassName("product-line-price")).forEach(function (item) {
		isUpdate = item.value.slice(1);
		item.value = paymentSign + isUpdate;
	});

	recalculateCart();
}

var isPaymentEl = document.getElementById("choices-payment-currency");
var choices = new Choices(isPaymentEl, {
	searchEnabled: false
});

// Profile Img
document
	.querySelector("#profile-img-file-input")
	.addEventListener("change", function () {
		var preview = document.querySelector(".user-profile-image");
		var file = document.querySelector(".profile-img-file-input").files[0];
		var reader = new FileReader();
		reader.addEventListener(
			"load",
			function () {
				preview.src = reader.result;
			},
			false
		);
		if (file) {
			reader.readAsDataURL(file);
		}
	});

flatpickr("#date-field", {
	enableTime: true,
	dateFormat: "d M, Y, h:i K",
});

isData();

function isData() {
	var plus = document.getElementsByClassName("plus"),
		minus = document.getElementsByClassName("minus");

	if (plus) {
		Array.from(plus).forEach(function (e) {
			e.onclick = function (event) {
				if (parseInt(e.previousElementSibling.value) < 10) {
					event.target.previousElementSibling.value++;

					var itemAmount = e.parentElement.parentElement.previousElementSibling.querySelector(".product-price").value;

					var priceselection = e.parentElement.parentElement.nextElementSibling.querySelector(".product-line-price");

					var productQty = e.parentElement.querySelector(".product-quantity").value;

					updateQuantity(productQty, itemAmount, priceselection);
				}
			}
		});

	}

	if (minus) {
		Array.from(minus).forEach(function (e) {
			e.onclick = function (event) {
				if (parseInt(e.nextElementSibling.value) > 1) {
					event.target.nextElementSibling.value--;
					var itemAmount = e.parentElement.parentElement.previousElementSibling.querySelector(".product-price").value;
					var priceselection = e.parentElement.parentElement.nextElementSibling.querySelector(".product-line-price");
					// var productQty = 1;
					var productQty = e.parentElement.querySelector(".product-quantity").value;
					updateQuantity(productQty, itemAmount, priceselection);
				}
			};
		});
	}
}

var count = 1;


function new_link() {
	if (typeof count === 'undefined') {
		debugger;
		count = 1;
	}
	count++;
	var tr1 = document.createElement("tr");
	tr1.id = count;
	tr1.className = "product";

	var delLink =
		"<tr>" +
		'<th scope="row" class="product-id">' +
		count +
		"</th>" +
		'<td class="text-start" style="vertical-align:top">' +
		'<div class="mb-2">' +
		'<input class="form-control" type="text" name="Qty" id="productName-' + count + '">' +
		'</div>' +
		"</td>" +
		"<td>" +
		'<select class="form-control product-price" name="Rcode-' + count + '" type="number"  onchange="updateDefectOptions(this.value, ' + count + ')" id="productRate-' + count + '">' +
		'<option value="R1">R 1</option>' +
		'<option value="R2">R 2</option>' +
		'<option value="R3">R 3</option>' +
		'<option value="R4">R 4</option>' +
		'<option value="R5">R 5</option>' +
		'<option value="R6">R 6</option>' +
		'<option value="R7">R 7</option>' +
		'<option value="R8">R 8</option>' +
		'<option value="R9">R 9</option>' +
		'<option value="R10">R 10</option>' +
		'<option value="R11">R 11</option>' +
		'<option value="R12">R 12</option>' +
		'<option value="R13">R 13</option>' +
		'<option value="R14">R 14</option>' +
		'<option value="R15">R 15</option>' +
		'<option value="R16">R 16</option>' +
		'<option value="R17">R 17</option>' +
		'<option value="R18">R 18</option>' +
		'<option value="R19">R 19</option>' +
		'<option value="R20">R 20</option>' +
		'<option value="R21">R 21</option>' +
		'<option value="R22">R 22</option>' +
		'<option value="R23">R 23</option>' +
		'<option value="R23">R 23</option>' +
		'<option value="R24">R 24</option>' +
		'<option value="R25">R 25</option>' +
		'<option value="R26">R 26</option>' +
		'<option value="R27">R 27</option>' +
		'</select>' +
		"</td>" +
		"<td>" +
		'<div class="">' +
		'<select class="form-control" name="Defect-' + count + '" id="productQty-' + count + '" >' +
		' <option value="Dimensional rejection in outer diameter">Dimensional rejection in outer diameter</option>'+
		'</select>' +
		"</div>" +
		"</td>" +
		'<td class="text-end">' +
		"<div>" +
		'<input type="text" class="form-control product-line-price" name="location-' + count + '" id="productLocation-' + count + '"/>' +
		"</div>" +
		"</td>" +
		'<td class="text-end">' +
		//"<div>" +
		//'<input type="text" class="form-control product-line-price" name="subqty-' + count + '" id="subqty-' + count + '"/>' +
		//"</div>" +
		"</td>" +
		'<td class="" style="vertical-align:top">' +
		'<button type="button" name="plusbtn-' + count + '" id="plusbtn-' + count + '" value="' + count + '" onclick="addSubRow(\'productRate-' + count + '\', \'productQty-' + count + '\', \'productLocation-' + count + '\',\'plusbtn-' + count + '\', ' + count + ')" class="btn btn-success">'
		+ '<input type="hidden" name="ButtonValue" value="' + count + '">'
		+ '<i class="ri-add-fill me-1 align-bottom"></i></button>' +
		"</td>" +
		'<td class="product-removal" style="vertical-align:top">' +
		'<a href="javascript:void(0)" class="btn btn-danger">Delete</a>'
	"</td>" +
		"</tr>";

	tr1.innerHTML = document.getElementById("newForm").innerHTML + delLink;
	document.getElementById("newlink").appendChild(tr1);

	var genericExamples = document.querySelectorAll("[data-trigger]");
	Array.from(genericExamples).forEach(function (genericExamp) {
		var element = genericExamp;
		new Choices(element, {
			placeholderValue: "This is a placeholder set in the config",
			searchPlaceholderValue: "This is a search placeholder",
		});
	});

	isData();
	remove();
	amountKeyup();
	//resetRow()
}


function addSubRow(id1, id2, id3, id5, count) {
	debugger;
	// unique sub-row ID generation
	var subRowCount = new Date().getTime();

	var parentElement1 = document.getElementById(id1);
	if (parentElement1) {
		var subRow1 = document.createElement("tr");
		subRow1.id = 'subRow-' + subRowCount + '-1';
		subRow1.innerHTML =
			'<td>' +
			'<select class="form-control product-line-price subRow-' + count + '" name="Rcode' + id1 + '" placeholder="RCode" onchange="updateDefectOptionsinside(this.value, ' + count + ')">' +
		'<option value="R1">R 1</option>' +
		'<option value="R2">R 2</option>' +
		'<option value="R3">R 3</option>' +
		'<option value="R4">R 4</option>' +
		'<option value="R5">R 5</option>' +
		'<option value="R6">R 6</option>' +
		'<option value="R7">R 7</option>' +
		'<option value="R8">R 8</option>' +
		'<option value="R9">R 9</option>' +
		'<option value="R10">R 10</option>' +
		'<option value="R11">R 11</option>' +
		'<option value="R12">R 12</option>' +
		'<option value="R13">R 13</option>' +
		'<option value="R14">R 14</option>' +
		'<option value="R15">R 15</option>' +
		'<option value="R16">R 16</option>' +
		'<option value="R17">R 17</option>' +
		'<option value="R18">R 18</option>' +
		'<option value="R19">R 19</option>' +
		'<option value="R20">R 20</option>' +
		'<option value="R21">R 21</option>' +
		'<option value="R22">R 22</option>' +
		'<option value="R23">R 23</option>' +
		'<option value="R23">R 23</option>' +
		'<option value="R24">R 24</option>' +
		'<option value="R25">R 25</option>' +
		'<option value="R26">R 26</option>' +
		'<option value="R27">R 27</option>' +
			'</select>' +
			'<td>';
		parentElement1.parentNode.insertBefore(subRow1, parentElement1.nextSibling);
	}

	var parentElement2 = document.getElementById(id2);
	if (parentElement2) {
		var subRow2 = document.createElement("tr");
		subRow2.id = 'subRow-' + subRowCount + '-2';
		subRow2.innerHTML =
			'<td>' +
			'<select class="form-control product-line-price subRow-' + count + '" id="insidedes-' + count + '" name="Defect-' + count + '" placeholder="Defect">' +
			'</select>' +
			'<td>';
		parentElement2.parentNode.insertBefore(subRow2, parentElement2.nextSibling);
	}

	var parentElement3 = document.getElementById(id3);
	if (parentElement3) {
		var subRow3 = document.createElement("tr");
		subRow3.id = 'subRow-' + subRowCount + '-3';
		subRow3.innerHTML =
			'<td><input type="text" class="form-control subRow-' + count + '" name="Location' + id3 + '" product-line-price" /></td>';
		parentElement3.parentNode.insertBefore(subRow3, parentElement3.nextSibling);
	}

	//var parentElement4 = document.getElementById(id4);
	//if (parentElement4) {
	//	var subRow4 = document.createElement("tr");
	//	subRow4.id = 'subRow-' + subRowCount + '-4';
	//	subRow4.innerHTML =
	//		'<td><input type="text" class="form-control subRow-' + count + '" name="subqty' + id4 + '" /></td>';
	//	parentElement4.parentNode.insertBefore(subRow4, parentElement4.nextSibling);
	//}

	var parentElement5 = document.getElementById(id5);
	if (parentElement5) {
		var subRow5 = document.createElement("tr");
		subRow5.id = 'subRow-' + subRowCount + '-5';
		subRow5.innerHTML =
			'<td><button type="button" class="btn btn-danger subRow-' + count + '" onclick="removeSubRow(\'subRow-' + subRowCount + '\')">-</button></td>';
		parentElement5.parentNode.insertBefore(subRow5, parentElement5.nextSibling);
	}

	return false;
}

function removeSubRow(subRowCount) {
	debugger;
	for (var i = 1; i <= 5; i++) {
		var subRow = document.getElementById(subRowCount + '-' + i);
		if (subRow) {
			subRow.remove();
		}
	}
}

remove();
/* Set rates + misc */
var taxRate = 0.125;
var shippingRate = 65.0;
var discountRate = 0.15;

function remove() {
	Array.from(document.querySelectorAll(".product-removal a")).forEach(function (el) {
		el.addEventListener("click", function (e) {
			removeItem(e);
			resetRow()
		});
	});
}

//function resetRow() {

//	Array.from(document.getElementById("newlink").querySelectorAll("tr")).forEach(function (subItem, index) {
//		var incid = index + 1;
//		subItem.querySelector('.product-id').innerHTML = incid;

//	});
//}


function updateDescription(selectedValue) {
	debugger
	var defectDropdown = $('.description');

	// Clear existing options
	defectDropdown.empty();

	// Make an AJAX request to get data from the server based on the selected value
	$.ajax({
		url: "/GetDescription/MRB",
		method: 'GET',
		data: { Rcode: selectedValue },
		success: function (data) {
			defectDropdown.append($('<option>', {
				value: data,
				text: data
			}));
		},
		error: function (error) {
			console.error('Error fetching data:', error);
		}
	});
}
/* Recalculate cart */
function recalculateCart() {
	var subtotal = 0;

	Array.from(document.getElementsByClassName("product")).forEach(function (item) {
		Array.from(item.getElementsByClassName("product-line-price")).forEach(function (e) {
			if (e.value) {
				subtotal += parseFloat(e.value.slice(1));
			}
		});
	});

	/* Calculate totals */
	var tax = subtotal * taxRate;
	var discount = subtotal * discountRate;

	var shipping = subtotal > 0 ? shippingRate : 0;
	var total = subtotal + tax + shipping - discount;

	document.getElementById("cart-subtotal").value =
		paymentSign + subtotal.toFixed(2);
	document.getElementById("cart-tax").value = paymentSign + tax.toFixed(2);
	document.getElementById("cart-shipping").value =
		paymentSign + shipping.toFixed(2);
	document.getElementById("cart-total").value = paymentSign + total.toFixed(2);
	document.getElementById("cart-discount").value =
		paymentSign + discount.toFixed(2);
	document.getElementById("totalamountInput").value =
		paymentSign + total.toFixed(2);
	document.getElementById("amountTotalPay").value =
		paymentSign + total.toFixed(2);
}

function amountKeyup() {

	// var listArray = [];

	// listArray.push(document.getElementsByClassName('product-price'));
	Array.from(document.getElementsByClassName('product-price')).forEach(function (item) {
		item.addEventListener('keyup', function (e) {

			var priceselection = item.parentElement.nextElementSibling.nextElementSibling.querySelector('.product-line-price');

			var amount = e.target.value;
			var itemQuntity = item.parentElement.nextElementSibling.querySelector('.product-quantity').value;

			updateQuantity(amount, itemQuntity, priceselection);

		});
	});
}

amountKeyup();
/* Update quantity */
function updateQuantity(amount, itemQuntity, priceselection) {
	var linePrice = amount * itemQuntity;
	/* Update line price display and recalc cart totals */
	linePrice = linePrice.toFixed(2);
	priceselection.value = paymentSign + linePrice;

	recalculateCart();

}

/* Remove item from cart */
function removeItem(removeButton) {
	removeButton.target.closest("tr").remove();
	recalculateCart();
}

//Choise Js
var genericExamples = document.querySelectorAll("[data-trigger]");
Array.from(genericExamples).forEach(function (genericExamp) {
	var element = genericExamp;
	new Choices(element, {
		placeholderValue: "This is a placeholder set in the config",
		searchPlaceholderValue: "This is a search placeholder",
	});
});

//Address
function billingFunction() {
	if (document.getElementById("same").checked) {
		document.getElementById("shippingName").value =
			document.getElementById("billingName").value;
		document.getElementById("shippingAddress").value =
			document.getElementById("billingAddress").value;
		document.getElementById("shippingPhoneno").value =
			document.getElementById("billingPhoneno").value;
		document.getElementById("shippingTaxno").value =
			document.getElementById("billingTaxno").value;
	} else {
		document.getElementById("shippingName").value = "";
		document.getElementById("shippingAddress").value = "";
		document.getElementById("shippingPhoneno").value = "";
		document.getElementById("shippingTaxno").value = "";
	}
}


var cleaveBlocks = new Cleave('#cardNumber', {
	blocks: [4, 4, 4, 4],
	uppercase: true
});

var genericExamples = document.querySelectorAll('[data-plugin="cleave-phone"]');
Array.from(genericExamples).forEach(function (genericExamp) {
	var element = genericExamp;
	new Cleave(element, {
		delimiters: ['(', ')', '-'],
		blocks: [0, 3, 3, 4]
	});
});

let viewobj;
var invoices_list = localStorage.getItem("invoices-list");
var options = localStorage.getItem("option");
var invoice_no = localStorage.getItem("invoice_no");
var invoices = JSON.parse(invoices_list);

if (localStorage.getItem("invoice_no") === null && localStorage.getItem("option") === null) {
	viewobj = '';
	var value = "#VL" + Math.floor(11111111 + Math.random() * 99999999);
	document.getElementById("invoicenoInput").value = value;
} else {
	viewobj = invoices.find(o => o.invoice_no === invoice_no);
}

// Invoice Data Load On Form
if ((viewobj != '') && (options == "edit-invoice")) {

	document.getElementById("registrationNumber").value = viewobj.company_details.legal_registration_no;
	document.getElementById("companyEmail").value = viewobj.company_details.email;
	document.getElementById('companyWebsite').value = viewobj.company_details.website;
	new Cleave("#compnayContactno", {
		prefix: viewobj.company_details.contact_no,
		delimiters: ['(', ')', '-'],
		blocks: [0, 3, 3, 4]
	});
	document.getElementById("companyAddress").value = viewobj.company_details.address;
	document.getElementById("companyaddpostalcode").value = viewobj.company_details.zip_code;

	var preview = document.querySelectorAll(".user-profile-image");
	if (viewobj.img !== '') {
		preview.src = viewobj.img;
	}

	document.getElementById("invoicenoInput").value = "#VAL" + viewobj.invoice_no;
	document.getElementById("invoicenoInput").setAttribute('readonly', true);
	document.getElementById("date-field").value = viewobj.date;
	document.getElementById("choices-payment-status").value = viewobj.status;
	document.getElementById("totalamountInput").value = "$" + viewobj.order_summary.total_amount;

	document.getElementById("billingName").value = viewobj.billing_address.full_name;
	document.getElementById("billingAddress").value = viewobj.billing_address.address;
	new Cleave("#billingPhoneno", {
		prefix: viewobj.company_details.contact_no,
		delimiters: ['(', ')', '-'],
		blocks: [0, 3, 3, 4]
	});
	document.getElementById("billingTaxno").value = viewobj.billing_address.tax;

	document.getElementById("shippingName").value = viewobj.shipping_address.full_name;
	document.getElementById("shippingAddress").value = viewobj.shipping_address.address;
	new Cleave("#shippingPhoneno", {
		prefix: viewobj.company_details.contact_no,
		delimiters: ['(', ')', '-'],
		blocks: [0, 3, 3, 4]
	});

	document.getElementById("shippingTaxno").value = viewobj.billing_address.tax;

	var paroducts_list = viewobj.prducts;
	var counter = 1;
	do {
		counter++;
		if (paroducts_list.length > 1) {
			document.getElementById("add-item").click();
		}
	} while (paroducts_list.length - 1 >= counter);

	var counter_1 = 1;

	setTimeout(() => {
		Array.from(paroducts_list).forEach(function (element) {
			document.getElementById("productName-" + counter_1).value = element.product_name;
			document.getElementById("productDetails-" + counter_1).value = element.product_details;
			document.getElementById("productRate-" + counter_1).value = element.rates;
			document.getElementById("product-qty-" + counter_1).value = element.quantity;
			document.getElementById("productPrice-" + counter_1).value = "$" + ((element.rates) * (element.quantity));
			counter_1++;
		});
	}, 300);

	document.getElementById("cart-subtotal").value = "$" + viewobj.order_summary.sub_total;
	document.getElementById("cart-tax").value = "$" + viewobj.order_summary.estimated_tex;
	document.getElementById("cart-discount").value = "$" + viewobj.order_summary.discount;
	document.getElementById("cart-shipping").value = "$" + viewobj.order_summary.shipping_charge;
	document.getElementById("cart-total").value = "$" + viewobj.order_summary.total_amount;

	document.getElementById("choices-payment-type").value = viewobj.payment_details.payment_method;
	document.getElementById("cardholderName").value = viewobj.payment_details.card_holder_name;

	var cleave = new Cleave('#cardNumber', {
		prefix: viewobj.payment_details.card_number,
		delimiter: ' ',
		blocks: [4, 4, 4, 4],
		uppercase: true
	});
	document.getElementById("amountTotalPay").value = "$" + viewobj.order_summary.total_amount;

	document.getElementById("exampleFormControlTextarea1").value = viewobj.notes;

}

document.addEventListener("DOMContentLoaded", function () {
	// //Form Validation
	var formEvent = document.getElementById('invoice_form');
	var forms = document.getElementsByClassName('needs-validation');

	// Loop over them and prevent submission
	formEvent.addEventListener("submit", function (event) {
		event.preventDefault();

		// get fields value
		var i_no = (document.getElementById("invoicenoInput").value).slice(4);
		var email = document.getElementById("companyEmail").value;
		var date = document.getElementById("date-field").value;
		var invoice_amount = (document.getElementById("totalamountInput").value).slice(1);
		var status = document.getElementById("choices-payment-status").value;
		var billing_address_full_name = document.getElementById("billingName").value;
		var billing_address_address = document.getElementById("billingAddress").value;
		var billing_address_phone = (document.getElementById("billingPhoneno").value).replace(/[^0-9]/g, "");
		var billing_address_tax = document.getElementById("billingTaxno").value;
		var shipping_address_full_name = document.getElementById("shippingName").value;
		var shipping_address_address = document.getElementById("shippingAddress").value;
		var shipping_address_phone = (document.getElementById("shippingPhoneno").value).replace(/[^0-9]/g, "");
		var shipping_address_tax = document.getElementById("shippingTaxno").value;
		var payment_details_payment_method = document.getElementById("choices-payment-type").value;
		var payment_details_card_holder_name = document.getElementById("cardholderName").value;
		var payment_details_card_number = (document.getElementById("cardNumber").value).replace(/[^0-9]/g, "");
		var payment_details_total_amount = (document.getElementById("amountTotalPay").value).slice(1);
		var company_details_legal_registration_no = (document.getElementById("registrationNumber").value).replace(/[^0-9]/g, "");
		var company_details_email = document.getElementById("companyEmail").value;
		var company_details_website = document.getElementById('companyWebsite').value;
		var company_details_contact_no = (document.getElementById("compnayContactno").value).replace(/[^0-9]/g, "");
		var company_details_address = document.getElementById("companyAddress").value;
		var company_details_zip_code = document.getElementById("companyaddpostalcode").value;
		var order_summary_sub_total = (document.getElementById("cart-subtotal").value).slice(1);
		var order_summary_estimated_tex = (document.getElementById("cart-tax").value).slice(1);
		var order_summary_discount = (document.getElementById("cart-discount").value).slice(1);
		var order_summary_shipping_charge = (document.getElementById("cart-shipping").value).slice(1);
		var order_summary_total_amount = (document.getElementById("cart-total").value).slice(1);
		var notes = document.getElementById("exampleFormControlTextarea1").value;

		// get product value and make array
		var products = document.getElementsByClassName("product");
		var count = 1;
		var new_product_obj = [];
		Array.from(products).forEach(element => {
			var product_name = element.querySelector("#productName-" + count).value;
			var product_details = element.querySelector("#productDetails-" + count).value;
			var product_rate = parseInt(element.querySelector("#productRate-" + count).value);
			var product_qty = parseInt(element.querySelector("#product-qty-" + count).value);
			var product_price = (element.querySelector("#productPrice-" + count).value).split("$");;

			var product_obj = {
				product_name: product_name,
				product_details: product_details,
				rates: product_rate,
				quantity: product_qty,
				amount: parseInt(product_price[1])
			}
			new_product_obj.push(product_obj);
			count++;
		});

		if (formEvent.checkValidity() === false) {
			formEvent.classList.add("was-validated");
		} else {
			if ((options == "edit-invoice") && (invoice_no == i_no)) {
				objIndex = invoices.findIndex((obj => obj.invoice_no == i_no));

				invoices[objIndex].invoice_no = i_no;
				invoices[objIndex].customer = billing_address_full_name;
				invoices[objIndex].img = '';
				invoices[objIndex].email = email;
				invoices[objIndex].date = date;
				invoices[objIndex].invoice_amount = invoice_amount;
				invoices[objIndex].status = status;
				invoices[objIndex].billing_address = {
					full_name: billing_address_full_name,
					address: billing_address_address,
					phone: billing_address_phone,
					tax: billing_address_tax
				};
				invoices[objIndex].shipping_address = {
					full_name: shipping_address_full_name,
					address: shipping_address_address,
					phone: shipping_address_phone,
					tax: shipping_address_tax
				};
				invoices[objIndex].payment_details = {
					payment_method: payment_details_payment_method,
					card_holder_name: payment_details_card_holder_name,
					card_number: payment_details_card_number,
					total_amount: payment_details_total_amount
				};
				invoices[objIndex].company_details = {
					legal_registration_no: company_details_legal_registration_no,
					email: company_details_email,
					website: company_details_website,
					contact_no: company_details_contact_no,
					address: company_details_address,
					zip_code: company_details_zip_code
				};
				invoices[objIndex].order_summary = {
					sub_total: order_summary_sub_total,
					estimated_tex: order_summary_estimated_tex,
					discount: order_summary_discount,
					shipping_charge: order_summary_shipping_charge,
					total_amount: order_summary_total_amount,
				};
				invoices[objIndex].prducts = new_product_obj;
				invoices[objIndex].notes = notes;

				localStorage.removeItem("invoices-list");
				localStorage.removeItem("option");
				localStorage.removeItem("invoice_no");
				localStorage.setItem("invoices-list", JSON.stringify(invoices));
			} else {
				var new_data_object = {
					invoice_no: i_no,
					customer: billing_address_full_name,
					img: '',
					email: email,
					date: date,
					invoice_amount: invoice_amount,
					status: status,
					billing_address: {
						full_name: billing_address_full_name,
						address: billing_address_address,
						phone: billing_address_phone,
						tax: billing_address_tax
					},
					shipping_address: {
						full_name: shipping_address_full_name,
						address: shipping_address_address,
						phone: shipping_address_phone,
						tax: shipping_address_tax
					},
					payment_details: {
						payment_method: payment_details_payment_method,
						card_holder_name: payment_details_card_holder_name,
						card_number: payment_details_card_number,
						total_amount: payment_details_total_amount
					},
					company_details: {
						legal_registration_no: company_details_legal_registration_no,
						email: company_details_email,
						website: company_details_website,
						contact_no: company_details_contact_no,
						address: company_details_address,
						zip_code: company_details_zip_code
					},
					order_summary: {
						sub_total: order_summary_sub_total,
						estimated_tex: order_summary_estimated_tex,
						discount: order_summary_discount,
						shipping_charge: order_summary_shipping_charge,
						total_amount: order_summary_total_amount
					},
					prducts: new_product_obj,
					notes: notes
				};
				localStorage.setItem("new_data_object", JSON.stringify(new_data_object));
			}
			window.location.href = "apps-invoices-list.html";
		}
	});
});
