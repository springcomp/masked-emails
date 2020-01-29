import { fakeAsync, TestBed } from "@angular/core/testing";
import { HashService } from './hash.service';
import { RandomService, MockRandomService } from './random.service';
import { when } from 'q';

beforeEach(() => {
    TestBed.configureTestingModule({
        providers: [
            {
                provide: RandomService, useClass: MockRandomService
            }
        ]
    });
});

describe('HashService.hashPassword', () => {

    it('should have a deterministic random generator', () => {
        const hashService: HashService = TestBed.get(HashService);
        const n = hashService.h();
        expect(n).toBe(18);
    });

    it('should convert UTF-8 strings to byte array', () =>{
        const hashService: HashService = TestBed.get(HashService);
        var u = hashService.convertUtf8StringToByteArray('élément');
        expect(u.buffer).toEqual(new Uint8Array([195, 169, 108, 195, 169, 109, 101, 110, 116]).buffer);
    });

    it('should convert arrays to base64', () => {
        const hashService: HashService = TestBed.get(HashService);
        var b = hashService.toBase64(new Uint8Array([195, 169, 108, 195, 169, 109, 101, 110, 116]));
        expect(b).toBe('w6lsw6ltZW50');
    });

    it('should return pseudo random salt array', () => {

        const hashService: HashService = TestBed.get(HashService);
        var s = hashService.randomSalt(4);
        expect(s.buffer).toEqual(new Uint8Array([54, 85, 76, 48]).buffer);
    });

    it('should hash plain text password', () => {

        const hashService: HashService = TestBed.get(HashService);

        const password = 'plaintext';
        const hashed = hashService.hashPassword(password)

        expect(hashed).toBe('JClY1QtvRV/YfYhb05OaW1jj4J2sNo6lG6DxkAX5MBt88N8ZJcSkfqf5LAjm5x9ign24ThyyGNPWK54/8nIaXjZVTDA=');
    });

});
