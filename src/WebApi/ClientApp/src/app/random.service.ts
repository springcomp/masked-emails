import { Injectable } from "@angular/core";

export interface IRandomService {
    getNext(maxValue: number): number;
}

@Injectable({
    providedIn: 'root'
})
export class RandomService implements IRandomService {
    public getNext(maxValue: number): number {
        return this.randomIntFromInterval(0, maxValue);
    }
    private randomIntFromInterval(min: number, max: number): number { // min and max included 
        return Math.floor(Math.random() * (max - min + 1) + min);
    }
}

@Injectable({
    providedIn: 'root'
})
export class MockRandomService implements IRandomService {
    private next: number = 0;
    private sequence: number[] = [18, 42, 33, 12, 21, 3, 63, 1, 0];

    public getNext(maxValue: number): number {
        return this.sequence[this.next++];
    }
}
