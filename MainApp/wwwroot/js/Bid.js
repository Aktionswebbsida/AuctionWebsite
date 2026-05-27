//const connection = new signalR.HubConnectionBuilder()
//    .withUrl(`https://localhost:7284/bid?adId=${currentAdId}`)
//    .withAutomaticReconnect()
//    .build();




//connection.on("UpdateHighestBid", function (adId, amount, userName) {
//    const bidDisplay = document.getElementById("highestBid_" + adId);
//    if (bidDisplay) {
//        bidDisplay.innerText = new Intl.NumberFormat('en-US', {
//            style: 'currency',
//            currency: 'USD',
//            maximumFractionDigits: 0
//        }).format(amount);

//        bidDisplay.style.color = "#28a745";
//        setTimeout(() => { bidDisplay.style.color = ""; }, 3000);
//    }

//    const historyList = document.getElementById("bidHistory");
//    if (historyList) {
//        const noBidsMessage = document.getElementById("noBidsMessage");
//        if (noBidsMessage) noBidsMessage.remove();

//        const newEntry = document.createElement("li");
//        newEntry.className = "list-group-item bg-success text-white border-0 mb-1 rounded";

//        const formattedAmount = new Intl.NumberFormat('en-US', {
//            style: 'currency',
//            currency: 'USD',
//            maximumFractionDigits: 0
//        }).format(amount);

//        newEntry.innerHTML = `<strong>${userName}</strong> just bidded <strong>${formattedAmount}</strong>`;
//        historyList.prepend(newEntry);

//        if (historyList.children.length > 5) {
//            historyList.lastElementChild.remove();
//        }
//    }
//});

//connection.start()
//    .then(() => console.log("SignalR Connected"))
//    .catch(err => console.error(err.toString()));

//document.getElementById("bidForm").addEventListener("submit", async function (e) {
//    e.preventDefault();

//    const formData = new FormData(this);
//    const errorDiv = document.getElementById("errorMessages");
//    const bidInput = document.getElementById("bidAmountInput");

//    errorDiv.innerText = "";

//    try {
//        const response = await fetch(window.location.href, {
//            method: "POST",
//            body: formData,
//            headers: {
//                "X-Requested-With": "XMLHttpRequest",
//                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
//            }
//        });

//        if (response.ok) {
//            bidInput.value = "";
//            errorDiv.innerText = "";
//        } else {
//            const errorText = await response.text();
//            console.log("Server error content:", errorText);


//            if (errorText.includes("<!DOCTYPE html>")) {
//                errorDiv.innerText = "Bid failed: Server returned an invalid response.";
//            } else {

//                errorDiv.innerText = errorText || "Bid was too low or invalid!";
//            }
//        }
//    } catch (err) {
//        errorDiv.innerText = "Connection error.";
//        console.error("Fetch error:", err);
//    }
//});

const connection = new signalR.HubConnectionBuilder().withUrl(`https://localhost:7284/bid?adId=${currentAdId}`).withAutomaticReconnect().build();

connection.on("UpdateHighestBid", function (adId, amount, UserName) {
    const biddisplay = document.getElementById("highestBid_" + adId);
        if(biddisplay){
            biddisplay.innerText = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
                maximumFractionDigits: 0
            }).format(amount);

            biddisplay.style.color = "#28a745"
            setTimeout(() => {
                biddisplay.style.color = "";
            }, 3000)
    }

    const historyList = document.getElementById("bidHistory");
    if (historyList) {
        const nobidmessage = document.getElementById("noBidsMessage");
        if (nobidmessage) nobidmessage.remove();

        const newEntry = document.createElement("li");
        newEntry.className = "list-group-item bg-success text-white border-0 mb-1 rounded";

       const formattedAmount = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
            maximumFractionDigits: 0
        }).format(amount);

        newEntry.innerHTML = ` <strong>${UserName}</strong> just bidded <strong>${formattedAmount}</strong>`;

        historyList.prepend(newEntry);

        if (historyList.children.length > 5) {
            historyList.lastElementChild.remove();
        }
    }
});

let auctionTimer;
let winnerProcess;

function CleanUi() {
    const bidform = document.getElementById("bidForm")
    if (bidform) {
        const button = bidform.querySelector("button");
        const input = bidform.querySelector("input");
        if (button) {
            button.disabled = true;
            button.innerHTML = "Auction Closed"
        }
        if (input) {
            input.disabled = true;
        }
    }
}
function StartCountDown(seconds) {
    if (auctionTimer) {
        clearInterval(auctionTimer);
    }
    auctionTimer = setInterval( function () {
      
        const display = document.getElementById("timeDisplay");
        if (display) {
            display.innerHTML = seconds + "s"
        }
          
            

            if (seconds <= 0) {
                clearInterval(auctionTimer);
                if (display) display.innerHTML = "0s"
                if (!winnerProcess) {
                    
                    winnerProcess = true;
                    triggerwinning(currentAdId);
                }
              
              
               
           
        }
        if (seconds > 0) {
            seconds--;
        }
    }, 1000);
}


//async function triggerwinning(adid) {
//    winnerProcess = true;
//    try {
//        // Anropa din AuctionApi direkt på port 7284
//        const response = await fetch(`https://localhost:7284/bids/winner/${adid}`, {
//            method: "POST",
//            headers: {
//                "X-Requested-With": "XMLHttpRequest",
//                "Content-Type": "application/json"
//            }
//        });

//        if (response.ok) {

//            console.log("Success: Vinnare korad!");


//        } else {
//            const errorText = await response.text();
//            console.error("API Error (Status " + response.status + "):", errorText);
//        }
//    } catch (err) {
//        winnerProcess = false;
//        console.error("Connection error:", err);
//    }
//}

async function triggerwinning(adid) {
    winnerProcess = true;
    try {
        const response = await fetch(`https://localhost:7284/bids/winner/${adid}`, {
            method: "POST",
          
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "Content-Type":"application/json"
                
            }


        });

        if (response.ok) {
            console.log("Winner");
        }
        else {
            const ErrorText = await response.text();
            console.log("Server error content", ErrorText);

           
        }
    } catch (err) {
        winnerProcess = false;
       
        console.error("Fetch error");
    }
}
connection.on("YouWon", function (winner) {

    if (auctionTimer) clearInterval(auctionTimer);
    winnerProcess = true;
    CleanUi();
    const biddisplay = document.getElementById("highestBid_" + winner.adId);
    if (biddisplay) {
        biddisplay.innerText = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
            maximumFractionDigits: 0
        }).format(winner.bidAmount);

        biddisplay.style.color = "#28a745"
       
        

        

     
        setTimeout(() => {
            biddisplay.style.color = "";
        }, 3000)
    }
    const TimeDisplay = document.getElementById("timeDisplay");
    if (TimeDisplay) TimeDisplay.innerHTML = "00:00:00";
    //let auctionStatus = document.getElementById("auctionStatus");

    //if (!auctionStatus) {

    //    const bidContainer = document.getElementById("bidContainer");

    //    auctionStatus = document.createElement("div");
    //    auctionStatus.id = "auctionStatus";

    //    auctionStatus.innerHTML = `
    //        <div class="alert alert-danger shadow-sm mt-3">
    //            auction slut
    //            <div id="winnerDisplay"
    //                 class="list-group-item bg-success text-white border-0 mb-1 rounded mt-2">
    //                <strong>Congrats ${winner.userName}</strong>
    //            </div>
    //        </div>
    //    `;

    //    if (bidContainer) {
    //        bidContainer.replaceWith(auctionStatus);
    //    }
    //}

    let auctionStatus = document.getElementById("auctionStatus");

    if (!auctionStatus) {
        const bidContainer = document.getElementById("bidContainer");

        auctionStatus = document.createElement("div");
        auctionStatus.id = "auctionStatus";

        auctionStatus.innerHTML = `<div class="alert alert-danger shadow-sm mt-2">
        auction slut 
          <div id="winnerDisplay" class="list-group-item bg-success text-white border-0 mb-1 rounded mt-2">
                                    <strong> Congrats ${winner.userName}</strong>
                                </div>
                                </div>
                                `
                               

        if (bidContainer) {
            bidContainer.replaceWith(auctionStatus);
        }

    }
    console.log("Winner", winner.userName)
    
});


connection.start().then(() => console.log("SignalR connected")).catch(err => console.error(err.toString()));

const bidform = document.getElementById("bidForm");
if (bidform) {
    bidform.addEventListener("submit", async function (e) {
        e.preventDefault();
        const formData = new FormData(this);
        const errormessageDiv = document.getElementById("errorMessages");
        const bidInput = document.getElementById("bidAmountInput");

        errormessageDiv.innerText = "";

        try {
            const response = await fetch(window.location.href, {
                method: "POST",
                body: formData,
                headers: {
                    "X-Requested-With": "XMLHttpRequest",
                    "RequestVerificationToken": document.querySelector('input[name = "__RequestVerificationToken"]').value
                }


            });

            if (response.ok) {
                bidInput.value = "";
                errormessageDiv.innerText = "";
            }
            else {
                const ErrorText = await response.text();
                console.log("Server error content", ErrorText);

                if (ErrorText.includes("<!DOCTYPE html>")) {
                    errormessageDiv.innerText = "Bid Failed";
                }
                else {
                    errormessageDiv.innerText = ErrorText || "Bid was too low";
                }
            }
        } catch (err) {
            errormessageDiv.innerText = "Connection Failed";
            console.error("Fetch error", err)
        }
    });
}
