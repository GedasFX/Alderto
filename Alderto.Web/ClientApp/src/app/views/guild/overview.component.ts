import { Component, OnInit } from '@angular/core';
import { GuildService, CurrencyService, AldertoWebGuildPreferencesApi } from 'src/app/services';
import { Observable, from, of } from 'rxjs';
import { GuildConfiguration } from 'src/app/models';
import { switchMap, take } from 'rxjs/operators';
import { ILeaderboardEntry } from 'src/app/models/leaderboard-entry';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './overview.component.html'
})
export class OverviewComponent implements OnInit {
  public config: GuildConfiguration;
  public leaderborard: ILeaderboardEntry[];

  public userIsAdmin: boolean;

  public configForm: FormGroup;

  public editing = {} as any;

  constructor(
    private readonly guildService: GuildService,
    private readonly guildPreferencesApi: AldertoWebGuildPreferencesApi,
    private readonly currencyService: CurrencyService,
    private readonly formBuilder: FormBuilder,
    private readonly toastr: ToastrService
  ) { }

  public ngOnInit() {
    this.configForm = this.formBuilder.group({
      prefix: [null, [Validators.required, Validators.maxLength(20)]],
      currencySymbol: [null, [Validators.required, Validators.maxLength(50)]],
      timelyRewardQuantity: [null, Validators.required],
      timelyCooldown: [null, Validators.required]
    });

    this.guildService.currentGuild$.subscribe(async g => {
      if (!g) {
        return;
      }

      this.userIsAdmin = g.userIsAdmin;

      this.config = await g.preferences;
      this.configForm.setValue({
        prefix: [this.config.prefix],
        currencySymbol: [this.config.currencySymbol],
        timelyRewardQuantity: [this.config.timelyRewardQuantity],
        timelyCooldown: [this.config.timelyCooldown]
      });

      this.currencyService.getTop50().pipe(take(1)).subscribe(c => {
        this.leaderborard = c;
      });
    });
  }

  public setEditing(key: string, editing = true) {
    this.editing[key] = editing;

    this.configForm.markAsUntouched();
    this.configForm.markAsPristine();
  }

  public enableEdit(key: string) {
    this.setEditing(key);
  }

  public confirmEdit(key: string) {
    if (this.configForm.controls.prefix.invalid) {
      return;
    }

    const preferences = {} as GuildConfiguration;
    preferences[key] = this.configForm.value[key];

    this.guildPreferencesApi.updatePreferences(this.guildService.currentGuildId, preferences).subscribe(() => {
      this.config[key] = preferences[key];

      this.toastr.success('Preference was updated successfully!', 'Success!');
      this.setEditing(key, false);
    });
  }

  public cancelEdit(key: string) {
    this.setEditing(key, false);
  }
}
