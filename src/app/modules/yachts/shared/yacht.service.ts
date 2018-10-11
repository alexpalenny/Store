import { Observable, of, throwError as observableThrowError } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConfig } from '../../../config/app.config';
import { Yacht } from './yacht.model';
import { catchError, tap, map } from 'rxjs/operators';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { _ } from '@biesbjerg/ngx-translate-extract/dist/utils/utils';
import { LoggerService } from '../../../core/services/logger.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class YachtService {
  yachtsUrl: string;

  static checkIfUserCanVote(): boolean {
    return Number(localStorage.getItem('votes')) < AppConfig.votesLimit;
  }

  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private snackBar: MatSnackBar) {
    this.yachtsUrl = AppConfig.endpoints.yachts;
  }

  private static handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      LoggerService.log(`${operation} failed: ${error.message}`);

      if (error.status >= 500) {
        throw error;
      }

      return of(result as T);
    };
  }

  getYachts(): Observable<Yacht[]> {
    return this.http.get<Yacht[]>(this.yachtsUrl);
  }

  getYachtById(id: string): Observable<Yacht> {
    // const url = `${this.yachtsUrl}/${id}`;
    // return this.http.get<Yacht>(url).pipe(
    //   tap(() => LoggerService.log(`fetched yacht id=${id}`)),
    //   catchError(YachtService.handleError<Yacht>(`getYacht id=${id}`))
    // );
    var index = +id - 1;
    return this.http.get<Yacht>(this.yachtsUrl).pipe(
      map(val => val[index])
    );

  }

  createYacht(yacht: Yacht): Observable<Yacht> {
    return this.http.post<Yacht>(this.yachtsUrl, JSON.stringify({
      name: yacht.name
    }), httpOptions).pipe(
      tap((yachtSaved: Yacht) => {
        LoggerService.log(`added yacht w/ id=${yachtSaved.id}`);
        this.showSnackBar('yachtCreated');
      }),
      catchError(YachtService.handleError<Yacht>('addYacht'))
    );
  }

  deleteYachtById(id: any): Observable<Array<Yacht>> {
    const url = `${this.yachtsUrl}/${id}`;

    return this.http.delete<Array<Yacht>>(url, httpOptions).pipe(
      tap(() => LoggerService.log(`deleted yacht id=${id}`)),
      catchError(YachtService.handleError<Array<Yacht>>('deleteYacht'))
    );
  }

  like(yacht: Yacht) {
    if (YachtService.checkIfUserCanVote()) {
      const url = `${this.yachtsUrl}/${yacht.id}/like`;
      return this.http
        .post(url, {}, httpOptions)
        .pipe(
          tap(() => {
            LoggerService.log(`updated yacht id=${yacht.id}`);
            localStorage.setItem('votes', '' + (Number(localStorage.getItem('votes')) + 1));
            yacht.length += 1;
            this.showSnackBar('saved');
          }),
          catchError(YachtService.handleError<any>('updateYacht'))
        );
    } else {
      this.showSnackBar('yachtLikeMaximum');
      return observableThrowError('maximum votes');
    }
  }

  showSnackBar(name): void {
    this.translateService.get([String(_('yachtCreated')), String(_('saved')),
    String(_('yachtLikeMaximum')), String(_('yachtRemoved'))], { 'value': AppConfig.votesLimit }).subscribe((texts) => {
      const config: any = new MatSnackBarConfig();
      config.duration = AppConfig.snackBarDuration;
      this.snackBar.open(texts[name], 'OK', config);
    });
  }
}
