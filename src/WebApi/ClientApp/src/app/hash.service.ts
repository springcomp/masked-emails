import { Injectable } from "@angular/core";
import { RandomService } from './random.service';
import * as sha512 from 'js-sha512';

@Injectable({
    providedIn: 'root'
})
export class HashService {

    private salt_chars: string = "$-()@&.,;!?/0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    constructor(
        private randomService: RandomService
    ) { }

    public hashPassword(plaintext: string): string {

        const salt = this.randomSalt(4);

        var hash = sha512.sha512.create();
        hash.update(plaintext);
        hash.update(salt);
        const hashed = hash.array();

        // https://stackoverflow.com/questions/14071463/how-can-i-merge-typedarrays-in-javascript

        var array = new Uint8Array(salt.length + hashed.length);
        array.set(hashed);
        array.set(salt, hashed.length);

        return this.toBase64(array);
    }

    // for testing purposes only
    public h(): number { return this.randomService.getNext(66); }

    // there methods are public for testing purposes only

    public randomSalt(length: number): Uint8Array {
        const bytes = this.convertUtf8StringToByteArray(this.salt_chars);
        var array = new Uint8Array(length);
        for (var index = 0; index < length; index++) {
            array[index] = bytes[this.randomService.getNext(bytes.length)];
        }
        return array;
    }

    public convertUtf8StringToByteArray(str: string): Uint8Array {
        const utf8 = unescape(encodeURIComponent(str));
        var array = new Uint8Array(utf8.length);
        for (var index = 0; index < array.length; index++) {
            array[index] = utf8.charCodeAt(index);
        }
        return array;
    }

    public toBase64(array: Uint8Array): string {

        // https://gist.github.com/enepomnyaschih/72c423f727d395eeaa09697058238727

        const base64abc = (() => {
            let abc = [],
                A = "A".charCodeAt(0),
                a = "a".charCodeAt(0),
                n = "0".charCodeAt(0);
            for (let i = 0; i < 26; ++i) {
                abc.push(String.fromCharCode(A + i));
            }
            for (let i = 0; i < 26; ++i) {
                abc.push(String.fromCharCode(a + i));
            }
            for (let i = 0; i < 10; ++i) {
                abc.push(String.fromCharCode(n + i));
            }
            abc.push("+");
            abc.push("/");
            return abc;
        })();

        function bytesToBase64(bytes: Uint8Array) {

            let result: string = ''
            let i: number
            let l: number = bytes.length;

            console.log(bytes[i - 2]);

            for (i = 2; i < l; i += 3) {
                result += base64abc[bytes[i - 2] >> 2];
                result += base64abc[((bytes[i - 2] & 0x03) << 4) | (bytes[i - 1] >> 4)];
                result += base64abc[((bytes[i - 1] & 0x0F) << 2) | (bytes[i] >> 6)];
                result += base64abc[bytes[i] & 0x3F];
            }
            if (i === l + 1) { // 1 octet missing
                result += base64abc[bytes[i - 2] >> 2];
                result += base64abc[(bytes[i - 2] & 0x03) << 4];
                result += "==";
            }
            if (i === l) { // 2 octets missing
                result += base64abc[bytes[i - 2] >> 2];
                result += base64abc[((bytes[i - 2] & 0x03) << 4) | (bytes[i - 1] >> 4)];
                result += base64abc[(bytes[i - 1] & 0x0F) << 2];
                result += "=";
            }
            return result;
        }

        return bytesToBase64(array);
    }
}
