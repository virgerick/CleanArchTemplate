async function encrypt(plainText, password){
    let enc = new TextEncoder();
    let encoded = enc.encode(plainText);
 
    let key =  window.crypto.subtle.importKey(
        "raw",
        enc.encode(password),
        { name: "AES-GCM", length: 256 },
        false,
        ["encrypt", "decrypt"]
    );
    console.log({
        plainText, password, enc, encoded, key
    })
    let iv = window.crypto.getRandomValues(new Uint8Array(12));

    let encryptedContent =  window.crypto.subtle.encrypt(
        { name: "AES-GCM", iv: iv },
        key,
        encoded
    );

    let combined = new Uint8Array(iv.length + encryptedContent.byteLength);
    combined.set(iv);
    combined.set(new Uint8Array(encryptedContent), iv.length);

    return combined.toString();
};
async function decrypt(combinedString, password){
        let enc = new TextEncoder();
        let combined = Uint8Array.from(combinedString.split(','));

        let iv = combined.slice(0, 12);
        let encryptedContent = combined.slice(12);

        let key = await window.crypto.subtle.importKey(
            "raw",
            enc.encode(password),
            { name: "AES-GCM", length: 256 },
            false,
            ["encrypt", "decrypt"]
        );

        let decryptedContent = await window.crypto.subtle.decrypt(
            { name: "AES-GCM", iv: iv },
            key,
            encryptedContent
        );

        let dec = new TextDecoder();
        return dec.decode(decryptedContent);
    }

